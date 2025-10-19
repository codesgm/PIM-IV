class ChatWidget {
    constructor(config = {}) {
        this.config = {
            proxyUrl: config.proxyUrl || 'http://192.168.0.152:9000',
            pollingInterval: config.pollingInterval || 3000,
            maxMessageLength: config.maxMessageLength || 500,
            welcomeMessage: config.welcomeMessage || 'Olá! Como posso ajudá-lo hoje?',
            showEscalationButton: config.showEscalationButton !== false
        };
        
        this.isOpen = false;
        this.sessionId = null;
        this.chatId = null; // Só definido após escalação
        this.chatState = 'AI_ACTIVE'; // AI_ACTIVE, ESCALATING, HUMAN_ASSIGNED
        this.lastMessageId = 0;
        this.userData = null;
        this.pollingInterval = null;
        
        this.init();
    }
    
    init() {
        this.createWidget();
        this.loadUserData();
        this.loadSession();
    }
    
    createWidget() {
        // Widget HTML
        const widgetHTML = `
            <div id="chat-widget" class="chat-widget">
                <div id="chat-button" class="chat-button">
                    <i class="fas fa-comments"></i>
                    <span id="chat-badge" class="chat-badge" style="display: none;">0</span>
                </div>
                
                <div id="chat-container" class="chat-container">
                    <div class="chat-header">
                        <div class="chat-title">
                            <i class="fas fa-headset"></i>
                            <span>Suporte MidTalk</span>
                            <div id="chat-state-indicator" class="chat-state-indicator state-ai">
                                🤖 IA Ativa
                            </div>
                        </div>
                        <button id="chat-close" class="chat-close">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>
                    
                    <div id="identification-modal" class="identification-modal">
                        <div class="identification-content">
                            <h3>Identificação</h3>
                            <p>Para iniciar o atendimento, precisamos de algumas informações:</p>
                            <form id="identification-form">
                                <div class="form-group">
                                    <label for="user-name">Nome completo *</label>
                                    <input type="text" id="user-name" required minlength="2">
                                </div>
                                <div class="form-group">
                                    <label for="user-email">E-mail *</label>
                                    <input type="email" id="user-email" required>
                                </div>
                                <button type="submit" class="btn-primary">Iniciar Chat</button>
                            </form>
                        </div>
                    </div>
                    
                    <div id="chat-messages" class="chat-messages"></div>
                    
                    <div id="chat-loading" class="chat-loading" style="display: none;">
                        <div class="loading-dots">
                            <span></span><span></span><span></span>
                        </div>
                        <span>IA está pensando...</span>
                    </div>
                    
                    <div class="chat-input">
                        <textarea id="chat-input-field" placeholder="Digite sua mensagem..." rows="1"></textarea>
                        <button id="chat-send" disabled>
                            <i class="fas fa-paper-plane"></i>
                        </button>
                    </div>
                    
                    <div id="escalation-panel" class="escalation-panel" style="display: none;">
                        <button id="escalate-btn" class="escalate-btn">
                            👨‍💻 Falar com técnico
                        </button>
                    </div>
                </div>
            </div>
        `;
        
        document.body.insertAdjacentHTML('beforeend', widgetHTML);
        this.bindEvents();
    }
    
    bindEvents() {
        this.elements = {
            widget: document.getElementById('chat-widget'),
            button: document.getElementById('chat-button'),
            container: document.getElementById('chat-container'),
            close: document.getElementById('chat-close'),
            messages: document.getElementById('chat-messages'),
            input: document.getElementById('chat-input-field'),
            sendButton: document.getElementById('chat-send'),
            loading: document.getElementById('chat-loading'),
            badge: document.getElementById('chat-badge'),
            identificationModal: document.getElementById('identification-modal'),
            identificationForm: document.getElementById('identification-form'),
            userNameInput: document.getElementById('user-name'),
            userEmailInput: document.getElementById('user-email'),
            stateIndicator: document.getElementById('chat-state-indicator'),
            escalationPanel: document.getElementById('escalation-panel'),
            escalateBtn: document.getElementById('escalate-btn')
        };
        
        // Event listeners
        this.elements.button.addEventListener('click', () => this.toggleChat());
        this.elements.close.addEventListener('click', () => this.closeChat());
        this.elements.sendButton.addEventListener('click', () => this.sendMessage());
        this.elements.escalateBtn.addEventListener('click', () => this.requestEscalation());
        this.elements.identificationForm.addEventListener('submit', (e) => this.handleIdentification(e));
        
        this.elements.input.addEventListener('keypress', (e) => {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                this.sendMessage();
            }
        });
        
        this.elements.input.addEventListener('input', () => {
            this.autoResize();
            this.elements.sendButton.disabled = !this.elements.input.value.trim();
        });
    }
    
    toggleChat() {
        if (this.isOpen) {
            this.closeChat();
        } else {
            this.openChat();
        }
    }
    
    openChat() {
        this.isOpen = true;
        this.elements.container.classList.add('open');
        this.elements.button.style.display = 'none';
        this.clearBadge();
        
        if (!this.userData) {
            this.showIdentificationForm();
        } else if (!this.sessionId) {
            this.startChat();
        }
    }
    
    closeChat() {
        this.isOpen = false;
        this.elements.container.classList.remove('open');
        this.elements.button.style.display = 'flex';
    }
    
    showIdentificationForm() {
        this.elements.identificationModal.classList.add('show');
        this.elements.userNameInput.focus();
    }
    
    hideIdentificationForm() {
        this.elements.identificationModal.classList.remove('show');
    }
    
    async handleIdentification(e) {
        e.preventDefault();
        
        const name = this.elements.userNameInput.value.trim();
        const email = this.elements.userEmailInput.value.trim();
        
        if (!this.validateUserData(name, email)) {
            return;
        }
        
        this.userData = { name, email };
        this.saveUserData();
        this.hideIdentificationForm();
        await this.startChat();
    }
    
    validateUserData(name, email) {
        if (name.length < 2) {
            alert('Nome deve ter pelo menos 2 caracteres');
            return false;
        }
        
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            alert('E-mail inválido');
            return false;
        }
        
        return true;
    }
    
    async startChat() {
        if (!this.userData) return;
        
        try {
            this.updateStatus('Conectando...');
            this.showLoading();
            
            const response = await fetch(`${this.config.proxyUrl}/api/proxy/start-chat`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    user_name: this.userData.name,
                    user_email: this.userData.email,
                    initial_message: `Usuário iniciou conversa via FAQ`
                })
            });
            
            if (!response.ok) throw new Error('Erro ao iniciar chat');
            
            const data = await response.json();
            this.sessionId = data.session_id;
            this.chatState = 'AI_ACTIVE';
            this.saveSession();
            
            this.hideLoading();
            this.addMessage('system', `Olá ${this.userData.name}! Conectando com nossa IA...`);
            this.updateStateIndicator();
            this.showEscalationPanel();
            
            // Processar resposta inicial da IA
            console.log('Resposta inicial recebida:', data);
            if (data.initial_response) {
                console.log('Processando initial_response:', data.initial_response);
                if (data.initial_response.type === 'ai') {
                    this.handleAIResponse(data.initial_response);
                } else if (data.initial_response.type === 'ai_with_escalation') {
                    this.handleAIResponse(data.initial_response);
                    this.addMessage('system', 'Posso escalar para um técnico se precisar de mais ajuda.');
                } else if (data.initial_response.type === 'escalated') {
                    this.handleEscalation(data.initial_response);
                }
            }
            
        } catch (error) {
            console.error('Erro ao iniciar chat:', error);
            this.hideLoading();
            this.addMessage('system', 'Erro ao conectar. Tente novamente.');
        }
    }
    
    async sendMessage() {
        const message = this.elements.input.value.trim();
        console.log('sendMessage chamado:', { message, sessionId: this.sessionId });
        if (!message || !this.sessionId) {
            console.log('Mensagem vazia ou sessionId não encontrado');
            return;
        }
        
        this.elements.input.value = '';
        this.elements.sendButton.disabled = true;
        this.autoResize();
        
        // Adicionar mensagem do usuário
        this.addMessage('user', message);
        
        try {
            this.showLoading();
            
            const response = await fetch(`${this.config.proxyUrl}/api/proxy/send-message`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    session_id: this.sessionId,
                    message: message
                })
            });
            
            if (!response.ok) {
                if (response.status === 404) {
                    // Sessão expirou, limpar e reiniciar
                    console.log('Sessão expirou, limpando localStorage');
                    this.clearSession();
                    this.sessionId = null;
                    this.addMessage('system', 'Sessão expirou. Reiniciando chat...');
                    await this.startChat();
                    return;
                }
                throw new Error('Erro ao enviar mensagem');
            }
            
            const data = await response.json();
            this.hideLoading();
            
            // Processar resposta baseada no tipo
            console.log('Resposta recebida:', data);
            if (data.type === 'ai') {
                this.handleAIResponse(data);
            } else if (data.type === 'ai_with_escalation_question') {
                this.handleAIResponse(data);
                this.showEscalationQuestion();
            } else if (data.type === 'escalation_question') {
                this.handleEscalationQuestion(data);
            } else if (data.type === 'escalated') {
                this.handleEscalation(data);
            } else if (data.type === 'human') {
                // Mensagem enviada para técnico, aguardar resposta
                this.addMessage('system', 'Mensagem enviada para o técnico...');
            }
            
        } catch (error) {
            console.error('Erro ao enviar mensagem:', error);
            this.hideLoading();
            this.addMessage('system', 'Erro ao enviar mensagem. Tente novamente.');
        } finally {
            this.elements.sendButton.disabled = false;
        }
    }
    
    handleAIResponse(data) {
        // Adicionar resposta da IA
        this.addMessage('ai', data.message, new Date(), data.confidence);
        
        // Mostrar botão de escalação se confidence baixa
        if (data.confidence < 0.7) {
            this.addMessage('system', 'Não consegui resolver completamente. Quer falar com um técnico?');
        }
    }
    
    handleEscalation(data) {
        this.chatState = 'HUMAN_ASSIGNED';
        this.chatId = data.chat_id;
        
        // Mostrar mensagem de transferência
        this.addMessage('system', data.message);
        this.addMessage('system', `Técnico será atribuído em breve. Chat ID: ${data.chat_id}`);
        
        // Atualizar interface
        this.updateStateIndicator();
        this.hideEscalationPanel();
        
        // Iniciar polling para mensagens do técnico
        this.startPolling();
    }
    
    handleEscalationQuestion(data) {
        // Adicionar pergunta do sistema
        this.addMessage('system', data.message);
        
        // Mostrar botões de sim/não
        this.showEscalationButtons();
    }
    
    showEscalationQuestion() {
        this.addMessage('system', 'Você deseja falar com um técnico?');
        this.showEscalationButtons();
    }
    
    showEscalationButtons() {
        const buttonsDiv = document.createElement('div');
        buttonsDiv.className = 'escalation-buttons';
        buttonsDiv.innerHTML = `
            <button class="btn-escalation-yes" onclick="window.chatWidget.confirmEscalation(true)">Sim</button>
            <button class="btn-escalation-no" onclick="window.chatWidget.confirmEscalation(false)">Não</button>
        `;
        this.elements.messages.appendChild(buttonsDiv);
        this.scrollToBottom();
    }
    
    confirmEscalation(confirm) {
        // Remover botões
        const buttons = document.querySelector('.escalation-buttons');
        if (buttons) buttons.remove();
        
        // Enviar resposta
        const response = confirm ? 'Sim' : 'Não';
        this.elements.input.value = response;
        this.sendMessage();
    }
    
    requestEscalation() {
        this.elements.input.value = "quero falar com técnico";
        this.sendMessage();
    }
    
    async startPolling() {
        if (this.pollingInterval) return; // Já está fazendo polling
        
        this.pollingInterval = setInterval(async () => {
            if (this.chatState === 'HUMAN_ASSIGNED' && this.chatId) {
                await this.pollMessages();
            }
        }, this.config.pollingInterval);
    }
    
    async pollMessages() {
        if (!this.sessionId || this.chatState !== 'HUMAN_ASSIGNED') return;
        
        try {
            const response = await fetch(`${this.config.proxyUrl}/api/proxy/messages/${this.sessionId}`);
            if (!response.ok) return;
            
            const data = await response.json();
            
            data.messages.forEach(msg => {
                if (msg.id > this.lastMessageId) {
                    const senderType = msg.sender_type.toLowerCase() === 'user' ? 'user' : 'technician';
                    
                    // Só adicionar mensagens do técnico no polling
                    if (senderType === 'technician') {
                        const messageDate = new Date(msg.created_at + 'Z');
                        this.addMessage(senderType, msg.message, messageDate);
                        
                        if (!this.isOpen) {
                            this.incrementBadge();
                        }
                    }
                    
                    this.lastMessageId = msg.id;
                }
            });
            
        } catch (error) {
            console.error('Erro ao buscar mensagens:', error);
        }
    }
    
    addMessage(type, text, timestamp = new Date(), confidence = null) {
        const messageDiv = document.createElement('div');
        messageDiv.className = `message ${type}`;
        
        const bubble = document.createElement('div');
        bubble.className = 'message-bubble';
        
        // Adicionar ícone baseado no tipo
        let icon = '';
        if (type === 'ai') {
            icon = '🤖 ';
            bubble.classList.add('ai-message');
        } else if (type === 'technician') {
            icon = '👨‍💻 ';
            bubble.classList.add('technician-message');
        } else if (type === 'system') {
            icon = 'ℹ️ ';
            bubble.classList.add('system-message');
        }
        
        bubble.innerHTML = icon + text;
        
        const time = document.createElement('div');
        time.className = 'message-time';
        time.textContent = this.formatTime(timestamp);
        
        // Adicionar confidence score para mensagens IA
        if (type === 'ai' && confidence !== null) {
            const confidenceDiv = document.createElement('div');
            confidenceDiv.className = 'confidence-score';
            confidenceDiv.textContent = `Confiança: ${Math.round(confidence * 100)}%`;
            messageDiv.appendChild(confidenceDiv);
        }
        
        messageDiv.appendChild(bubble);
        messageDiv.appendChild(time);
        
        this.elements.messages.appendChild(messageDiv);
        this.scrollToBottom();
    }
    
    updateStateIndicator() {
        const indicator = this.elements.stateIndicator;
        
        if (this.chatState === 'AI_ACTIVE') {
            indicator.textContent = '🤖 IA Ativa';
            indicator.className = 'chat-state-indicator state-ai';
        } else if (this.chatState === 'HUMAN_ASSIGNED') {
            indicator.textContent = '👨‍💻 Técnico Atribuído';
            indicator.className = 'chat-state-indicator state-human';
        }
    }
    
    showEscalationPanel() {
        if (this.config.showEscalationButton && this.chatState === 'AI_ACTIVE') {
            this.elements.escalationPanel.style.display = 'block';
        }
    }
    
    hideEscalationPanel() {
        this.elements.escalationPanel.style.display = 'none';
    }
    
    showLoading() {
        this.elements.loading.style.display = 'flex';
    }
    
    hideLoading() {
        this.elements.loading.style.display = 'none';
    }
    
    updateStatus(status) {
        // Pode ser usado para mostrar status na interface
        console.log('Status:', status);
    }
    
    scrollToBottom() {
        this.elements.messages.scrollTop = this.elements.messages.scrollHeight;
    }
    
    autoResize() {
        const input = this.elements.input;
        input.style.height = 'auto';
        input.style.height = Math.min(input.scrollHeight, 120) + 'px';
    }
    
    incrementBadge() {
        const badge = this.elements.badge;
        const count = parseInt(badge.textContent) + 1;
        badge.textContent = count;
        badge.style.display = 'block';
    }
    
    clearBadge() {
        this.elements.badge.style.display = 'none';
        this.elements.badge.textContent = '0';
    }
    
    // Métodos de persistência
    saveUserData() {
        if (this.userData) {
            localStorage.setItem('chat_user_data', JSON.stringify(this.userData));
        }
    }
    
    loadUserData() {
        const data = localStorage.getItem('chat_user_data');
        if (data) {
            this.userData = JSON.parse(data);
        }
    }
    
    saveSession() {
        if (this.sessionId) {
            localStorage.setItem('chat_session_id', this.sessionId);
            localStorage.setItem('chat_last_message_id', this.lastMessageId.toString());
            localStorage.setItem('chat_state', this.chatState);
            if (this.chatId) {
                localStorage.setItem('chat_id', this.chatId.toString());
            }
        }
    }
    
    loadSession() {
        this.sessionId = localStorage.getItem('chat_session_id');
        this.lastMessageId = parseInt(localStorage.getItem('chat_last_message_id') || '0');
        this.chatState = localStorage.getItem('chat_state') || 'AI_ACTIVE';
        const chatId = localStorage.getItem('chat_id');
        if (chatId) {
            this.chatId = parseInt(chatId);
        }
        
        // Se tem sessão ativa e foi escalado, iniciar polling
        if (this.sessionId && this.chatState === 'HUMAN_ASSIGNED') {
            this.updateStateIndicator();
            this.startPolling();
        }
    }
    
    clearSession() {
        localStorage.removeItem('chat_session_id');
        localStorage.removeItem('chat_last_message_id');
        localStorage.removeItem('chat_state');
        localStorage.removeItem('chat_id');
    }
    
    formatTime(date) {
        return date.toLocaleTimeString('pt-BR', {
            hour: '2-digit',
            minute: '2-digit'
        });
    }
}

// Inicializar widget quando DOM estiver pronto
document.addEventListener('DOMContentLoaded', function() {
    window.chatWidget = new ChatWidget({
        proxyUrl: 'http://192.168.0.152:9000'
    });
});
