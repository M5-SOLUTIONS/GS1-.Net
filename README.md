# ⬡M5-Storage — Sistema de Gestão de Recursos

> Plataforma de monitoramento e controle de suprimentos para bases espaciais, submarinos, plataformas de petróleo e ambientes de alta complexidade operacional.

---

## Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Diagrama de Arquitetura](#diagrama-de-arquitetura)
- [Diagrama de Entidade-Relacionamento](#diagrama-de-entidade-relacionamento)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Como Executar](#como-executar)
- [Exemplos de Testes](#exemplos-de-testes)
- [Funcionalidades](#funcionalidades)

---

## Sobre o Projeto

O **M5-Storage** foi desenvolvido para resolver um problema crítico em ambientes isolados: o controle preciso de recursos essenciais à sobrevivência da tripulação e operação da base.

O sistema gerencia recursos como **água, oxigênio, energia, medicamentos e alimentos**, registrando consumo e reabastecimento em tempo real. Quando um recurso atinge nível abaixo do mínimo seguro, um **alerta crítico é gerado automaticamente**, permitindo ação imediata da equipe.

**Aplicações reais:**
- Bases lunares e marcianas
- Estações orbitais
- Submarinos e plataformas de petróleo
- Bases militares e centros de pesquisa isolados

---

## Diagrama de Arquitetura

```
┌─────────────────────────────────────────────────────────┐
│                     CLIENT (Browser)                     │
└─────────────────────┬───────────────────────────────────┘
                      │ HTTP Request
┌─────────────────────▼───────────────────────────────────┐
│              ASP.NET CORE MVC (.NET 8)                   │
│                                                          │
│  ┌─────────────┐  ┌──────────────┐  ┌────────────────┐  │
│  │ Controllers │→ │    Models    │→ │     Views      │  │
│  │             │  │              │  │   (Razor/HTML) │  │
│  │ - Home      │  │ - Usuario    │  │                │  │
│  │ - Usuarios  │  │ - Recurso    │  │  Visual        │  │
│  │ - Recursos  │  │ - Recurso    │  │  Futurista     │  │
│  │ - Moviment. │  │   Energia    │  │  Dark Theme    │  │
│  │ - Alertas   │  │ - Recurso    │  │                │  │
│  └─────────────┘  │   Medicament │  └────────────────┘  │
│                   │ - Movimentac.│                       │
│                   │ - Alerta     │                       │
│                   └──────┬───────┘                       │
│                          │ Entity Framework Core 8       │
└──────────────────────────┼──────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────┐
│              Oracle Database (FIAP)                      │
│                                                          │
│  T_USUARIOS  T_RECURSOS  T_RECURSO_ENERGIA               │
│  T_RECURSO_MEDICAMENTO   T_MOVIMENTACOES  T_ALERTAS      │
└─────────────────────────────────────────────────────────┘
```

---

## Diagrama de Entidade-Relacionamento

```
┌──────────────────┐         ┌─────────────────────────────────┐
│   T_USUARIOS     │         │          T_RECURSOS              │
├──────────────────┤         ├─────────────────────────────────┤
│ PK id            │         │ PK id                            │
│    nome          │         │    nome                          │
│    email (UNIQUE)│         │    categoria (AGUA/OXIGENIO/     │
│    senha         │         │              ENERGIA/MEDICAMENTO/│
└────────┬─────────┘         │              ALIMENTO)           │
         │                   │    quantidade  NUMBER(18,2)      │
         │ 1                 │    minimo      NUMBER(18,2)      │
         │                   │    critico     NUMBER(1)         │
         │ N                 │    status      VARCHAR2(30)      │
┌────────▼──────────────┐    │    nivel       VARCHAR2(30)      │
│    T_MOVIMENTACOES    │    │    ultima_atualizacao TIMESTAMP  │
├───────────────────────┤    └──┬──────────────────────────────┘
│ PK id                 │       │ 1              │ 1
│ FK usuario_id ────────┘       │                │
│ FK recurso_id ────────────────┘                │
│    tipo_movimentacao          │                │
│    (CONSUMO/REABASTECIMENTO)  │                │
│    quantidade NUMBER(18,2)    │                │
│    descricao                  │ N              │ N
│    data_movimentacao          │                │
└───────────────────────┘  ┌───▼──────────┐ ┌──▼──────────────────┐
                           │  T_ALERTAS   │ │ T_RECURSO_ENERGIA    │
                           ├──────────────┤ ├─────────────────────┤
                           │ PK id        │ │ PK/FK id             │
                           │ FK recurso_id│ │    tipo_energia      │
                           │    mensagem  │ └─────────────────────┘
                           │    nivel     │
                           │    resolvido │ ┌─────────────────────┐
                           │    data_     │ │ T_RECURSO_MEDICAMENTO│
                           │    alerta    │ ├─────────────────────┤
                           └──────────────┘ │ PK/FK id             │
                                            │    validade          │
                                            └─────────────────────┘
```

**Relacionamentos:**
| Relação | Tipo | Regra |
|---|---|---|
| Usuario → Movimentacoes | 1:N | Restrict (não deleta usuário com movimentações) |
| Recurso → Movimentacoes | 1:N | Cascade delete |
| Recurso → Alertas | 1:N | Cascade delete |
| Recurso → RecursoEnergia | 1:1 | TPT (herança) |
| Recurso → RecursoMedicamento | 1:1 | TPT (herança) |

---

## Tecnologias Utilizadas

| Tecnologia | Versão | Uso |
|---|---|---|
| ASP.NET Core MVC | .NET 8 | Framework principal |
| Entity Framework Core | 8.0.27 | ORM / Migrations |
| Oracle.EntityFrameworkCore | 8.23.x | Driver Oracle |
| Oracle Database | ORCL (FIAP) | Banco de dados |
| Bootstrap | 5.x | Layout responsivo |
| Font Awesome | 6.5 | Ícones |

---

## Como Executar

### Pré-requisitos
- Visual Studio 2022 ou superior
- .NET 8 SDK

### Passo a passo

**1. Clone o repositório**
```bash
git clone [https://github.com/SEU_USUARIO/m5-storage.git](https://github.com/M5-SOLUTIONS/GS1-.Net.git)
cd m5-storage
```

**2. Configure a conexão com o banco**

Edite o arquivo `appsettings.json`:
```json
"ConnectionStrings": {
  "OracleConnection": "User Id=RM;Password=SENHA;Data Source=oracle.fiap.com.br:1521/ORCL;"
}
```

**3. Instale os pacotes NuGet**
```bash
dotnet restore
```

Ou pelo Package Manager Console:
```
Install-Package Microsoft.EntityFrameworkCore -Version 8.0.27
Install-Package Microsoft.EntityFrameworkCore.Relational -Version 8.0.27
Install-Package Oracle.EntityFrameworkCore -Version 8.23.26200
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.27
Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.27
```

**4. Crie e aplique as Migrations**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**5. Execute o projeto**
```bash
dotnet run
```

Acesse: `https://localhost:7100`

---

## Exemplos de Testes

### Teste 1 — Cadastrar um recurso crítico (gatilho de alerta automático)

1. Acesse **Recursos → Novo Recurso**
2. Preencha:
   - Nome: `Oxigênio Cabine A`
   - Categoria: `OXIGENIO`
   - Quantidade: `50`
   - Mínimo Seguro: `100`
3. Clique em **Registrar Recurso**
4. **Resultado esperado:** recurso criado com `Status = CRITICO` e um alerta automático gerado em **Alertas**

---

### Teste 2 — Registrar consumo e verificar atualização de estoque

1. Acesse **Movimentações → Nova Movimentação**
2. Preencha:
   - Recurso: `Oxigênio Cabine A`
   - Usuário: qualquer usuário cadastrado
   - Tipo: `CONSUMO`
   - Quantidade: `10`
3. Clique em **Registrar**
4. **Resultado esperado:** quantidade do recurso atualizada de 50 → 40

---

### Teste 3 — Reabastecimento resolve criticidade

1. Acesse **Movimentações → Nova Movimentação**
2. Preencha:
   - Recurso: `Oxigênio Cabine A`
   - Tipo: `REABASTECIMENTO`
   - Quantidade: `200`
3. Clique em **Registrar**
4. **Resultado esperado:** estoque vai para 240, `Status = NORMAL`, `Nivel = ALTO`

---

### Teste 4 — Resolver alerta

1. Acesse **Alertas**
2. Clique em **Resolver** no alerta aberto
3. **Resultado esperado:** alerta marcado como `RESOLVIDO` (linha fica esmaecida)

---

### Teste 5 — Validação de quantidade insuficiente

1. Tente consumir mais do que o estoque disponível
2. **Resultado esperado:** mensagem de erro `"Quantidade insuficiente em estoque!"`

---

## Funcionalidades

- CRUD completo de Usuários
- CRUD completo de Recursos (com herança TPT para Energia e Medicamento)
- Registro de Movimentações (consumo/reabastecimento) com atualização automática de estoque
- Geração automática de alertas quando recurso atinge nível crítico
- Resolução de alertas pela equipe
- Dashboard com contadores em tempo real
- Validação de estoque insuficiente
- Cálculo automático de nível (CRITICO / BAIXO / NORMAL / ALTO)
- Interface futurista

---

# Link Vídeos

## 1. Vídeo apresentação
```http
https://youtu.be/2WzsM5Qzcew
```

## 2. Vídeo pitch
```http
https://www.youtube.com/watch?v=6SMKNB_aJPI
```


---
# Desenvolvido por

- Guilherme Cintra RM562850
- Erick de Faria Gama RM561951
- Matheus Nascimento Corregio RM563765
- Pedro Fonseca de Almeida RM563466
- Daniel Fonseca de Almeida RM563045
