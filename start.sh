#!/bin/bash

echo "🚀 Iniciando aplicação PIM..."

# Para qualquer container rodando
echo "📦 Parando containers existentes..."
docker compose down

# Rebuild e sobe os containers
echo "🔨 Construindo e iniciando containers..."
docker compose build
docker compose up -d

# Aguarda os serviços ficarem prontos
echo "⏳ Aguardando serviços iniciarem..."
sleep 10

# Verifica status
echo "📊 Status dos containers:"
docker compose ps

# Lê o IP do arquivo .env
HOST_IP=$(grep "^HOST_IP=" .env | cut -d'=' -f2)

echo ""
echo "✅ Aplicação disponível em:"
echo "   Frontend: http://localhost:5028"
echo "   FAQ + Chat IA: http://localhost:5030"
echo "   Backend API: http://localhost:5000"
echo "   Proxy Service: http://localhost:9000"
echo "   Bot IA: http://localhost:8001"
echo ""
echo "🔑 Credenciais de login:"
echo "   Email: guilhermetts0@gmail.com"
echo "   Senha: admin123"
