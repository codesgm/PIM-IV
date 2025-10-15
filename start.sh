#!/bin/bash

echo "🚀 Iniciando aplicação PIM..."

# Para qualquer container rodando
echo "📦 Parando containers existentes..."
docker compose down

# Rebuild e sobe os containers
echo "🔨 Construindo e iniciando containers..."
docker compose up --build -d

# Aguarda os serviços ficarem prontos
echo "⏳ Aguardando serviços iniciarem..."
sleep 10

# Verifica status
echo "📊 Status dos containers:"
docker compose ps

echo ""
echo "✅ Aplicação disponível em:"
echo "   Frontend: http://localhost:5028"
echo "   Backend:  http://localhost:5000"
echo ""
echo "🔑 Credenciais de login:"
echo "   Email: guilhermetts0@gmail.com"
echo "   Senha: admin123"
