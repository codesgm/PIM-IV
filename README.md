# Sistema PIM - Gestão de Chamados e Suporte Técnico

## Arquitetura
- **Backend**: ASP.NET Core Web API (.NET 9.0)
- **Frontend**: ASP.NET Core MVC (.NET 9.0)  
- **Banco**: SQL Server 2022
- **Container**: Docker Compose
- **IA**: Google Gemini API + Chat Widget

## Estrutura do Projeto
```
PIM/
├── backend/          # API REST
├── frontend/         # Interface web
├── faq/             # FAQ público com chat IA
├── proxy/           # Proxy service (IA ↔ Técnico)
├── bot-ia/          # Serviço de IA (Gemini)
├── docker-compose.yml
├── .env             # Configurações (IP, URLs, etc.)
├── change-ip.sh     # Script para mudar IP
├── start.sh         # Script para iniciar
└── stop.sh          # Script para parar
```

## Configuração de IP

### Método 1: Script Automático (Recomendado)
```bash
# Alterar IP automaticamente
./change-ip.sh 192.168.1.100

# O script faz tudo:
# - Atualiza .env
# - Rebuild containers
# - Restart serviços
```

### Método 2: Manual
1. Editar arquivo `.env`:
```bash
HOST_IP=192.168.1.100  # ← Alterar aqui
```

2. Rebuild containers:
```bash
docker compose down
docker compose build
docker compose up -d
```

## Como Usar
```bash
# Iniciar aplicação
./start.sh

# Parar aplicação  
./stop.sh

# Alterar IP
./change-ip.sh <novo_ip>
```

## URLs (baseadas no HOST_IP do .env)
- **Frontend**: http://{HOST_IP}:5028
- **FAQ + Chat IA**: http://{HOST_IP}:5030
- **Backend API**: http://{HOST_IP}:5000
- **Proxy Service**: http://{HOST_IP}:9000
- **Bot IA**: http://{HOST_IP}:8001
- **SQL Server**: localhost:1433 (apenas local)

## Credenciais
- **Email**: guilhermetts0@gmail.com
- **Senha**: admin123

## Funcionalidades IA
✅ Chat widget com IA first
✅ Escalação inteligente para técnicos
✅ Confirmação antes de transferir
✅ Polling de mensagens em tempo real
✅ Gestão de estados (IA ↔ Humano)

## Status Atual
✅ Backend API funcionando
✅ Frontend web funcionando  
✅ FAQ com chat IA funcionando
✅ Sistema de escalação funcionando
✅ Configuração de IP centralizada
✅ Scripts de automação
⏳ Mobile app (próximo)
