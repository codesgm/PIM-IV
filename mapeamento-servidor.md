# Mapeamento para Servidor - MidTalk

## 🚀 Preparação para Deploy

### 1. Variáveis de Ambiente (.env)

```bash
# IP/Domínio do servidor
HOST_IP=seu-servidor.com.br
# ou
HOST_IP=192.168.1.100

# Portas (verificar disponibilidade no servidor)
FRONTEND_PORT=5028
FAQ_PORT=5030
BACKEND_PORT=5000
PROXY_PORT=9000
BOT_IA_PORT=8001
SQL_PORT=1433

# Banco de Dados
DB_SERVER=localhost,1433
DB_NAME=PimDatabase
DB_USER=sa
DB_PASSWORD=SuaSenhaSegura123!

# Google Gemini API
GOOGLE_API_KEY=sua_chave_gemini_aqui

# URLs internas (ajustar conforme HOST_IP)
BACKEND_URL=http://${HOST_IP}:${BACKEND_PORT}
PROXY_URL=http://${HOST_IP}:${PROXY_PORT}
BOT_IA_URL=http://${HOST_IP}:${BOT_IA_PORT}
```

### 2. Requisitos do Servidor

#### Sistema Operacional
- **Linux**: Ubuntu 20.04+ ou CentOS 8+
- **Windows**: Windows Server 2019+

#### Software Necessário
```bash
# Docker & Docker Compose
sudo apt update
sudo apt install docker.io docker-compose-plugin

# Git (para clone do projeto)
sudo apt install git

# Firewall (liberar portas)
sudo ufw allow 5028  # Frontend
sudo ufw allow 5030  # FAQ
sudo ufw allow 5000  # Backend
sudo ufw allow 9000  # Proxy
sudo ufw allow 8001  # Bot IA
sudo ufw allow 1433  # SQL Server
```

#### Recursos Mínimos
- **RAM**: 4GB (recomendado 8GB)
- **CPU**: 2 cores
- **Disco**: 20GB livres
- **Rede**: IP fixo ou domínio

### 3. Configurações de Rede

#### Firewall/Portas
```bash
# Portas que devem estar abertas
5028 - Frontend Web
5030 - FAQ + Chat IA
5000 - Backend API
9000 - Proxy Service
8001 - Bot IA
1433 - SQL Server (apenas interno)
```

#### DNS (se usar domínio)
```
A     midtalk.com.br          → IP_DO_SERVIDOR
CNAME faq.midtalk.com.br      → midtalk.com.br
CNAME api.midtalk.com.br      → midtalk.com.br
```

### 4. Ajustes nos Arquivos

#### docker-compose.yml
```yaml
# Verificar se as portas não conflitam
# Ajustar volumes para persistência
volumes:
  - ./data/sql:/var/opt/mssql/data  # Persistir banco
  - ./logs:/app/logs                # Logs da aplicação
```

#### Scripts de Deploy
```bash
# deploy.sh
#!/bin/bash
git pull origin main
./change-ip.sh $HOST_IP
docker compose down
docker compose build --no-cache
docker compose up -d
```

### 5. Configurações de Produção

#### Segurança
```bash
# SSL/HTTPS (usar nginx como proxy reverso)
sudo apt install nginx certbot python3-certbot-nginx

# Certificado SSL
sudo certbot --nginx -d midtalk.com.br -d faq.midtalk.com.br
```

#### Backup
```bash
# Script de backup do banco
#!/bin/bash
docker exec pim-sql-1 /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P $DB_PASSWORD \
  -Q "BACKUP DATABASE PimDatabase TO DISK = '/var/opt/mssql/backup/pim_$(date +%Y%m%d_%H%M%S).bak'"
```

#### Monitoramento
```bash
# Logs em tempo real
docker compose logs -f

# Status dos containers
docker compose ps

# Recursos do sistema
htop
df -h
```

### 6. Checklist de Deploy

#### Pré-Deploy
- [ ] Servidor configurado com Docker
- [ ] Portas liberadas no firewall
- [ ] Domínio apontando para o servidor (se aplicável)
- [ ] Chave da API do Google Gemini
- [ ] Backup do ambiente atual

#### Deploy
- [ ] Clone do repositório
- [ ] Configurar .env com dados do servidor
- [ ] Executar ./change-ip.sh com IP/domínio
- [ ] Testar conectividade das portas
- [ ] Executar docker compose up -d
- [ ] Verificar logs dos containers

#### Pós-Deploy
- [ ] Testar todas as URLs
- [ ] Verificar funcionamento do chat IA
- [ ] Testar criação de chamados
- [ ] Configurar backup automático
- [ ] Configurar SSL (se produção)

### 7. Comandos Úteis

```bash
# Verificar status
docker compose ps

# Ver logs
docker compose logs -f [service_name]

# Restart específico
docker compose restart frontend

# Backup manual
docker exec pim-sql-1 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $DB_PASSWORD -Q "BACKUP DATABASE PimDatabase TO DISK = '/var/opt/mssql/backup/backup.bak'"

# Restore
docker exec pim-sql-1 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $DB_PASSWORD -Q "RESTORE DATABASE PimDatabase FROM DISK = '/var/opt/mssql/backup/backup.bak'"
```

### 8. Troubleshooting

#### Problemas Comuns
```bash
# Container não inicia
docker compose logs [service_name]

# Porta em uso
sudo netstat -tulpn | grep :5028

# Conectividade
curl http://localhost:5028
telnet IP_SERVIDOR 5028

# Espaço em disco
df -h
docker system prune -a
```

### 9. Configuração com Domínio

#### nginx.conf (proxy reverso)
```nginx
server {
    listen 80;
    server_name midtalk.com.br;
    
    location / {
        proxy_pass http://localhost:5028;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
    }
}

server {
    listen 80;
    server_name faq.midtalk.com.br;
    
    location / {
        proxy_pass http://localhost:5030;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
    }
}
```

### 10. Variáveis Críticas

```bash
# Essenciais para funcionamento
HOST_IP=                    # IP ou domínio do servidor
GOOGLE_API_KEY=            # Chave da API do Gemini
DB_PASSWORD=               # Senha do SQL Server

# Opcionais (têm valores padrão)
FRONTEND_PORT=5028
FAQ_PORT=5030
BACKEND_PORT=5000
PROXY_PORT=9000
BOT_IA_PORT=8001
```

---

**Próximos Passos:**
1. Configurar servidor com Docker
2. Ajustar .env com dados reais
3. Executar deploy
4. Configurar SSL para produção
5. Implementar backup automático
