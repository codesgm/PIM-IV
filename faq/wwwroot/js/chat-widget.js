class ChatWidget {
    constructor(config = {}) {
        this.config = {
            proxyUrl: config.proxyUrl || 'http://localhost:9000',
            pollingInterval: config.pollingInterval || 3000,
            maxMessageLength: config.maxMessageLength || 500,
            welcomeMessage: config.welcomeMessage || 'Olá! Como posso ajudá-lo hoje?',
            ...config
        };
        
        this.isOpen = false;
        this.sessionId = null;
        this.lastMessageId = 0;
        this.pollingTimer = null;
        this.unreadCount = 0;
        this.userIdentified = false;
        this.userData = null;
        
        this.init();
    }
    
    init() {
        this.createWidget();
        this.bindEvents();
        this.loadSession();
        this.loadUserData();
    }
    
    createWidget() {
        const widget = document.createElement('div');
        widget.className = 'chat-widget';
        widget.innerHTML = `
            <div class="identification-modal" id="identificationModal">
                <div class="identification-form">
                    <div class="form-header">
                        <h3>Iniciar Conversa</h3>
                        <p>Para melhor atendimento, precisamos de algumas informações:</p>
                    </div>
                    <form id="identificationForm">
                        <div class="form-group">
                            <label for="userName">Nome completo *</label>
                            <input type="text" id="userName" class="form-input" placeholder="Seu nome completo" maxlength="100" required>
                            <div class="validation-error" id="nameError"></div>
                        </div>
                        <div class="form-group">
                            <label for="userEmail">Email *</label>
                            <input type="email" id="userEmail" class="form-input" placeholder="seu@email.com" maxlength="255" required>
                            <div class="validation-error" id="emailError"></div>
                        </div>
                        <div class="form-actions">
                            <button type="submit" class="form-button" id="startChatBtn">
                                <i class="fas fa-comments"></i>
                                Iniciar Chat
                            </button>
                        </div>
                    </form>
                </div>
            </div>
            <div class="chat-modal" id="chatModal">
                <div class="chat-header">
                    <div>
                        <div class="chat-title">Suporte MidTalk</div>
                        <div class="chat-status" id="chatStatus">Conectando...</div>
                    </div>
                    <button class="chat-close" id="chatClose">×</button>
                </div>
                <div class="chat-messages" id="chatMessages">
                    <div class="chat-loading" id="chatLoading">
                        <i class="fas fa-spinner fa-spin"></i>
                        <span style="margin-left: 8px;">Iniciando conversa...</span>
                    </div>
                </div>
                <div class="typing-indicator" id="typingIndicator">
                    <div class="typing-dots">
                        <div class="typing-dot"></div>
                        <div class="typing-dot"></div>
                        <div class="typing-dot"></div>
                    </div>
                    <span style="margin-left: 8px; font-size: 12px; color: #64748b;">Técnico digitando...</span>
                </div>
                <div class="chat-input">
                    <div class="input-group">
                        <textarea class="message-input" id="messageInput" 
                                placeholder="Digite sua mensagem..." 
                                rows="1" maxlength="${this.config.maxMessageLength}"></textarea>
                        <button class="send-button" id="sendButton">
                            <i class="fas fa-paper-plane"></i>
                        </button>
                    </div>
                </div>
            </div>
            <button class="chat-button" id="chatButton">
                <i class="fas fa-comments"></i>
                <div class="chat-badge" id="chatBadge">0</div>
            </button>
        `;
        
        document.body.appendChild(widget);
        this.elements = {
            button: document.getElementById('chatButton'),
            modal: document.getElementById('chatModal'),
            close: document.getElementById('chatClose'),
            messages: document.getElementById('chatMessages'),
            input: document.getElementById('messageInput'),
            sendButton: document.getElementById('sendButton'),
            status: document.getElementById('chatStatus'),
            loading: document.getElementById('chatLoading'),
            badge: document.getElementById('chatBadge'),
            typing: document.getElementById('typingIndicator'),
            identificationModal: document.getElementById('identificationModal'),
            identificationForm: document.getElementById('identificationForm'),
            userNameInput: document.getElementById('userName'),
            userEmailInput: document.getElementById('userEmail'),
            startChatBtn: document.getElementById('startChatBtn'),
            nameError: document.getElementById('nameError'),
            emailError: document.getElementById('emailError')
        };
    }
    
    bindEvents() {
        this.elements.button.addEventListener('click', () => this.toggleChat());
        this.elements.close.addEventListener('click', () => this.closeChat());
        this.elements.sendButton.addEventListener('click', () => this.sendMessage());
        
        this.elements.input.addEventListener('keypress', (e) => {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                this.sendMessage();
            }
        });
        
        this.elements.input.addEventListener('input', () => {
            this.autoResize();
        });
        
        // Event listeners para formulário de identificação
        this.elements.identificationForm.addEventListener('submit', (e) => {
            e.preventDefault();
            this.handleIdentificationSubmit();
        });
        
        this.elements.userNameInput.addEventListener('input', () => {
            this.clearError('name');
        });
        
        this.elements.userEmailInput.addEventListener('input', () => {
            this.clearError('email');
        });
    }
    
    autoResize() {
        const input = this.elements.input;
        input.style.height = 'auto';
        input.style.height = Math.min(input.scrollHeight, 80) + 'px';
    }
    
    async toggleChat() {
        if (this.isOpen) {
            this.closeChat();
        } else {
            await this.openChat();
        }
    }
    
    async openChat() {
        if (!this.userIdentified) {
            this.showIdentificationForm();
            return;
        }
        
        this.isOpen = true;
        this.elements.button.classList.add('active');
        this.elements.modal.classList.add('show');
        this.clearBadge();
        
        if (!this.sessionId) {
            await this.startChat();
        } else {
            this.startPolling();
        }
        
        this.elements.input.focus();
    }
    
    closeChat() {
        this.isOpen = false;
        this.elements.button.classList.remove('active');
        this.elements.modal.classList.remove('show');
        this.stopPolling();
    }
    
    async startChat() {
        try {
            this.updateStatus('Conectando...');
            
            const response = await fetch(`${this.config.proxyUrl}/api/proxy/start-chat`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    user_name: this.userData.name,
                    user_email: this.userData.email,
                    initial_message: 'Usuário iniciou conversa via FAQ'
                })
            });
            
            if (!response.ok) {
                throw new Error('Erro ao conectar com o suporte');
            }
            
            const data = await response.json();
            this.sessionId = data.session_id;
            this.saveSession();
            
            this.elements.loading.style.display = 'none';
            this.addMessage('system', `Olá ${this.userData.name}! ${this.config.welcomeMessage}`);
            this.updateStatus('Online');
            this.startPolling();
            
        } catch (error) {
            console.error('Erro ao iniciar chat:', error);
            this.showError('Não foi possível conectar ao suporte. Tente novamente.');
            this.updateStatus('Offline');
        }
    }
    
    async sendMessage() {
        const message = this.elements.input.value.trim();
        if (!message || !this.sessionId) return;
        
        this.elements.input.value = '';
        this.elements.sendButton.disabled = true;
        this.autoResize();
        
        // Adicionar mensagem na UI imediatamente
        this.addMessage('user', message);
        
        try {
            const response = await fetch(`${this.config.proxyUrl}/api/proxy/send-message`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    session_id: this.sessionId,
                    message: message
                })
            });
            
            if (!response.ok) {
                throw new Error('Erro ao enviar mensagem');
            }
            
        } catch (error) {
            console.error('Erro ao enviar mensagem:', error);
            this.addMessage('system', 'Erro ao enviar mensagem. Tente novamente.');
        } finally {
            this.elements.sendButton.disabled = false;
        }
    }
    
    async getMessages() {
        if (!this.sessionId) return;
        
        try {
            const response = await fetch(`${this.config.proxyUrl}/api/proxy/messages/${this.sessionId}`);
            
            if (!response.ok) {
                if (response.status === 404) {
                    // Sessão expirada
                    this.sessionId = null;
                    this.clearSession();
                    this.stopPolling();
                    this.showError('Sessão expirada. Inicie uma nova conversa.');
                    return;
                }
                throw new Error('Erro ao buscar mensagens');
            }
            
            const data = await response.json();
            
            data.messages.forEach(msg => {
                if (msg.id > this.lastMessageId) {
                    const senderType = msg.sender_type.toLowerCase() === 'user' ? 'user' : 'system';
                    this.addMessage(senderType, msg.message, new Date(msg.created_at));
                    this.lastMessageId = msg.id;
                    
                    // Incrementar badge se chat fechado
                    if (!this.isOpen && senderType === 'system') {
                        this.incrementBadge();
                    }
                }
            });
            
        } catch (error) {
            console.error('Erro ao buscar mensagens:', error);
        }
    }
    
    addMessage(type, text, timestamp = new Date()) {
        const messageDiv = document.createElement('div');
        messageDiv.className = `message ${type}`;
        
        const bubble = document.createElement('div');
        bubble.className = 'message-bubble';
        bubble.textContent = text;
        
        const time = document.createElement('div');
        time.className = 'message-time';
        time.textContent = this.formatTime(timestamp);
        
        messageDiv.appendChild(bubble);
        messageDiv.appendChild(time);
        
        this.elements.messages.appendChild(messageDiv);
        this.scrollToBottom();
    }
    
    showError(message) {
        const errorDiv = document.createElement('div');
        errorDiv.className = 'chat-error';
        errorDiv.innerHTML = `
            <i class="fas fa-exclamation-triangle"></i>
            <span style="margin-left: 8px;">${message}</span>
        `;
        this.elements.messages.appendChild(errorDiv);
        this.scrollToBottom();
    }
    
    scrollToBottom() {
        this.elements.messages.scrollTop = this.elements.messages.scrollHeight;
    }
    
    updateStatus(status) {
        this.elements.status.textContent = status;
    }
    
    startPolling() {
        this.stopPolling();
        this.pollingTimer = setInterval(() => {
            this.getMessages();
        }, this.config.pollingInterval);
    }
    
    stopPolling() {
        if (this.pollingTimer) {
            clearInterval(this.pollingTimer);
            this.pollingTimer = null;
        }
    }
    
    incrementBadge() {
        this.unreadCount++;
        this.elements.badge.textContent = this.unreadCount;
        this.elements.badge.classList.add('show');
    }
    
    clearBadge() {
        this.unreadCount = 0;
        this.elements.badge.classList.remove('show');
    }
    
    getUserName() {
        let userName = localStorage.getItem('chat_user_name');
        if (!userName) {
            userName = `Usuário_${Math.random().toString(36).substr(2, 9)}`;
            localStorage.setItem('chat_user_name', userName);
        }
        return userName;
    }
    
    saveSession() {
        if (this.sessionId) {
            localStorage.setItem('chat_session_id', this.sessionId);
            localStorage.setItem('chat_last_message_id', this.lastMessageId.toString());
        }
    }
    
    loadSession() {
        this.sessionId = localStorage.getItem('chat_session_id');
        this.lastMessageId = parseInt(localStorage.getItem('chat_last_message_id') || '0');
    }
    
    clearSession() {
        localStorage.removeItem('chat_session_id');
        localStorage.removeItem('chat_last_message_id');
    }
    
    formatTime(date) {
        return date.toLocaleTimeString('pt-BR', {
            hour: '2-digit',
            minute: '2-digit'
        });
    }
    
    // Métodos de identificação
    showIdentificationForm() {
        this.elements.identificationModal.classList.add('show');
        this.elements.userNameInput.focus();
        
        // Preencher com dados salvos se existirem
        const savedData = this.loadUserData();
        if (savedData) {
            this.elements.userNameInput.value = savedData.name || '';
            this.elements.userEmailInput.value = savedData.email || '';
        }
    }
    
    hideIdentificationForm() {
        this.elements.identificationModal.classList.remove('show');
    }
    
    validateUserData(name, email) {
        const errors = {};
        
        // Validar nome
        if (!name || name.trim().length < 2) {
            errors.name = 'Nome deve ter pelo menos 2 caracteres';
        } else if (name.trim().length > 100) {
            errors.name = 'Nome deve ter no máximo 100 caracteres';
        }
        
        // Validar email
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!email || !emailRegex.test(email)) {
            errors.email = 'Email deve ter um formato válido';
        } else if (email.length > 255) {
            errors.email = 'Email deve ter no máximo 255 caracteres';
        }
        
        return {
            valid: Object.keys(errors).length === 0,
            errors: errors
        };
    }
    
    showError(field, message) {
        const errorElement = field === 'name' ? this.elements.nameError : this.elements.emailError;
        const inputElement = field === 'name' ? this.elements.userNameInput : this.elements.userEmailInput;
        
        errorElement.textContent = message;
        errorElement.style.display = 'block';
        inputElement.classList.add('error');
    }
    
    clearError(field) {
        const errorElement = field === 'name' ? this.elements.nameError : this.elements.emailError;
        const inputElement = field === 'name' ? this.elements.userNameInput : this.elements.userEmailInput;
        
        errorElement.style.display = 'none';
        inputElement.classList.remove('error');
    }
    
    handleIdentificationSubmit() {
        const name = this.elements.userNameInput.value.trim();
        const email = this.elements.userEmailInput.value.trim();
        
        // Limpar erros anteriores
        this.clearError('name');
        this.clearError('email');
        
        // Validar dados
        const validation = this.validateUserData(name, email);
        
        if (!validation.valid) {
            // Mostrar erros
            if (validation.errors.name) {
                this.showError('name', validation.errors.name);
            }
            if (validation.errors.email) {
                this.showError('email', validation.errors.email);
            }
            return;
        }
        
        // Salvar dados e continuar
        this.saveUserData(name, email);
        this.hideIdentificationForm();
        this.openChat();
    }
    
    saveUserData(name, email) {
        const userData = { name, email };
        localStorage.setItem('chat_user_data', JSON.stringify(userData));
        this.userData = userData;
        this.userIdentified = true;
    }
    
    loadUserData() {
        try {
            const saved = localStorage.getItem('chat_user_data');
            if (saved) {
                const userData = JSON.parse(saved);
                // Validar dados salvos
                const validation = this.validateUserData(userData.name, userData.email);
                if (validation.valid) {
                    this.userData = userData;
                    this.userIdentified = true;
                    return userData;
                }
            }
        } catch (e) {
            console.warn('Erro ao carregar dados do usuário:', e);
        }
        
        this.userData = null;
        this.userIdentified = false;
        return null;
    }
}

// Inicializar widget quando DOM estiver pronto
document.addEventListener('DOMContentLoaded', function() {
    window.chatWidget = new ChatWidget({
        proxyUrl: 'http://localhost:9000'
    });
});
