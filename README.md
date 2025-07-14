# Registro de Atendimento - Clínica ACME

A Clínica ACME deseja uma solução web para registrar os atendimentos realizados com seus pacientes. O sistema deve
permitir o **cadastro de pacientes** e o **registro de atendimentos**, com funcionalidades de consulta, edição e
filtros.

---

## ✍️ Funcionalidades Principais

### 1. Cadastro de Pacientes

O cadastro deve conter os seguintes campos:

- **Nome** (obrigatório)
- **Data de nascimento** (obrigatório)
- **CPF** (obrigatório e único)
- **Sexo** (obrigatório)
- **Endereço completo:**
    - CEP
    - Cidade
    - Bairro
    - Endereço
    - Complemento (opcional)
- **Status** (obrigatório – Ativo/Inativo)

#### Requisitos:

- Listar pacientes com filtros por **nome**, **CPF** e **status**;
- Cadastrar e **editar** pacientes;
- **Inativar** pacientes;
- **Validação** para impedir CPF duplicado.

---

### 2. Registro de Atendimentos

Cada atendimento deve conter:

- **Paciente** (obrigatório)
- **Data e hora** (obrigatório – não pode ser no futuro)
- **Descrição do atendimento** (obrigatório, com suporte a textos longos e quebra de linha)
- **Status** (obrigatório – Ativo/Inativo)

#### Requisitos:

- Consultar pacientes, mostrando apenas os que estão **ativos**;
- Cadastrar e **editar** registros de atendimento;
- **Inativar** registros de atendimento;
- Listar atendimentos com filtro por:
    - **Data (período)**
    - **Paciente**
    - **Status**

---

## 🚀 Como Executar

### Pré-requisitos

- [Docker](https://www.docker.com/get-started) instalado
- [Docker Compose](https://docs.docker.com/compose/install/) instalado

### Execução com Docker

1. **Clone o repositório:**
   ```bash
   git clone <url-do-repositorio>
   cd RegistroDeAtendimento
   ```

2. **Execute o projeto com Docker Compose:**
   ```bash
   docker-compose up -d
   ```

3. **Aguarde a inicialização dos serviços:**
   - SQL Server (porta 1500)
   - API (porta 5223)
   - Aplicação Web (porta 5248)

4. **Acesse a aplicação:**
   - **Interface Web:** http://localhost:5248
   - **API:** http://localhost:5223
   - **SQL Server:** localhost:1500

### Estrutura dos Serviços

O projeto utiliza os seguintes containers:

- **sqlserver-dev:** Banco de dados SQL Server 2022
- **registrodeatendimento-api:** API REST em .NET
- **registrodeatendimento-web:** Interface web em Blazor

### Comandos Úteis

```bash
# Parar todos os serviços
docker-compose down

# Parar e remover volumes (dados do banco)
docker-compose down -v

# Ver logs dos serviços
docker-compose logs -f

# Reconstruir imagens
docker-compose up --build -d
```

### Configurações

- **Banco de dados:** SQL Server 2022
- **Senha do SA:** 1q2w3e4r@#$
- **Porta do SQL Server:** 1500
- **Porta da API:** 5223
- **Porta da Web:** 5248

---
