# Configuração Domínio midtalk.online

## 🌐 Estrutura de URLs

### URLs Finais (Produção)
```
https://midtalk.online/          → Landing Page (porta 5031)
https://midtalk.online/app       → Sistema principal (porta 5028)
https://midtalk.online/faq       → FAQ + Chat IA (porta 5030)
https://midtalk.online/api       → Backend API (porta 5000)
https://midtalk.online/proxy     → Proxy Service (porta 9000)
https://midtalk.online/bot       → Bot IA (porta 8001)
```

## ⚙️ Configuração Nginx

### Arquivo: `/etc/nginx/sites-available/midtalk`
```nginx
server {
    listen 80;
    server_name midtalk.online www.midtalk.online;

    # Frontend principal (/)
    location / {
        proxy_pass http://localhost:5028;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    # FAQ + Chat IA (/faq)
    location /faq {
        proxy_pass http://localhost:5030;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    # Backend API (/api)
    location /api {
        proxy_pass http://localhost:5000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    # Proxy Service (/proxy)
    location /proxy {
        proxy_pass http://localhost:9000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    # Bot IA (/bot)
    location /bot {
        proxy_pass http://localhost:8001;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

## 🔧 Variáveis de Ambiente (.env)

### Para Produção com Domínio
```bash
# Domínio
HOST_IP=midtalk.online

# Portas internas (não expostas)
LANDING_PORT=5031
FRONTEND_PORT=5028
FAQ_PORT=5030
BACKEND_PORT=5000
PROXY_PORT=9000
BOT_IA_PORT=8001
SQL_PORT=1433

# URLs com paths (para comunicação interna)
LANDING_URL=https://midtalk.online/
FRONTEND_URL=https://midtalk.online/app
BACKEND_URL=https://midtalk.online/api
PROXY_URL=https://midtalk.online/proxy
BOT_IA_URL=https://midtalk.online/bot
FAQ_URL=https://midtalk.online/faq

# Banco de dados
DB_SERVER=localhost,1433
DB_NAME=PimDatabase
DB_USER=sa
DB_PASSWORD=SuaSenhaSegura123!

# Google Gemini API
GOOGLE_API_KEY=sua_chave_gemini_aqui
```

## 📋 Comandos de Deploy

### 1. Instalar Nginx
```bash
sudo apt update
sudo apt install nginx
```

### 2. Configurar Site
```bash
# Copiar configuração
sudo cp nginx-config.conf /etc/nginx/sites-available/midtalk

# Ativar site
sudo ln -s /etc/nginx/sites-available/midtalk /etc/nginx/sites-enabled/

# Testar configuração
sudo nginx -t

# Restart nginx
sudo systemctl restart nginx
```

### 3. SSL com Certbot
```bash
# Instalar certbot
sudo apt install certbot python3-certbot-nginx

# Gerar certificado
sudo certbot --nginx -d midtalk.online -d www.midtalk.online

# Auto-renovação
sudo crontab -e
# Adicionar: 0 12 * * * /usr/bin/certbot renew --quiet
```

### 4. Atualizar Aplicação
```bash
# Atualizar .env
HOST_IP=midtalk.online

# Rebuild containers
./change-ip.sh midtalk.online

# Verificar funcionamento
curl https://midtalk.online
curl https://midtalk.online/faq
curl https://midtalk.online/api/health
```

## 🔄 Ajustes no Código

### Frontend (appsettings.json)
```json
{
  "ApiSettings": {
    "BaseUrl": "https://midtalk.online/api"
  }
}
```

### FAQ (config.js)
```javascript
const API_BASE_URL = 'https://midtalk.online/proxy';
const BOT_URL = 'https://midtalk.online/bot';
```

### Backend (appsettings.json)
```json
{
  "AllowedHosts": "midtalk.online;www.midtalk.online;localhost"
}
```

## 🎯 Checklist de Ativação

### DNS
- [ ] Apontar midtalk.online para IP do servidor
- [ ] Apontar www.midtalk.online para IP do servidor

### Servidor
- [ ] Nginx instalado e configurado
- [ ] Certificado SSL ativo
- [ ] Firewall liberado (80, 443)
- [ ] Docker containers rodando

### Aplicação
- [ ] .env atualizado com domínio
- [ ] URLs internas ajustadas
- [ ] Containers rebuilded
- [ ] Testes de conectividade

### Verificação
- [ ] https://midtalk.online (Landing Page)
- [ ] https://midtalk.online/app (Sistema principal)
- [ ] https://midtalk.online/faq (FAQ + Chat)
- [ ] https://midtalk.online/api/health (API)
- [ ] Chat IA funcionando
- [ ] Criação de chamados OK

---

**Nota**: Manter configuração local atual até ativação do domínio.
