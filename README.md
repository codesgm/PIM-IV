# Sistema PIM - Gestão de Chamados e Suporte Técnico

## Arquitetura
- **Backend**: ASP.NET Core Web API (.NET 9.0)
- **Frontend**: ASP.NET Core MVC (.NET 9.0)  
- **Banco**: SQL Server 2022
- **Container**: Docker Compose

## Estrutura do Projeto
```
PIM/
├── backend/          # API REST
├── frontend/         # Interface web
├── docker-compose.yml
├── start.sh          # Script para iniciar
└── stop.sh           # Script para parar
```

## Como Usar
```bash
# Iniciar aplicação
./start.sh

# Parar aplicação  
./stop.sh
```

## URLs
- **Frontend**: http://localhost:5028
- **Backend**: http://localhost:5000
- **SQL Server**: localhost:1433

## Credenciais
- **Email**: guilhermetts0@gmail.com
- **Senha**: admin123

## Status Atual
✅ Backend API funcionando (login, CRUD usuários)
✅ Frontend web funcionando (login, dashboard)
✅ Docker Compose configurado
✅ Scripts de start/stop
⏳ Próximos: Mobile app, sistema de chamados, IA
