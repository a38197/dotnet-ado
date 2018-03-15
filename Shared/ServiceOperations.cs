using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BidSoftware.Shared.DTODefinition;

namespace BidSoftware.Shared
{
    public interface IServiceOperations : IDisposable
    {
        /// <summary>
        /// Executa um teste para validação da coneção. É espectável que seja chamado este método antes de qualquer acesso à base de
        /// dados para validação da conexão aplicacional.
        /// </summary>
        void TestConnection();

        /// <summary>
        /// Valida as credenciais na tabela dos utilizadores, sp_validateUser
        /// </summary>
        /// <param name="creds">Credenciais a validar</param>
        /// <returns>true se as credenciais forem válidas</returns>
        bool IsValidUser(Credentials creds);

        /// <summary>
        /// Obtém um enumerado com X registos. Os dados de configuração estão no ficheiro de config. Todas as views para consulta contém um campo
        /// auxiliar ROW_NR que contém o número da linha ordenado pela chave
        /// </summary>
        /// <param name="table">Tabela</param>
        /// <param name="startRecord">Registo de início inclusivo</param>
        /// <param name="numRecords">Número de registos</param>
        /// <returns></returns>
        IEnumerable<IDtoObject> GetTablePage(DatabaseTable table, int startRecord, int numRecords);

        /// <summary>
        /// Devolve o registo da Tabela cuja chave seja a passada no parâmetro.
        /// </summary>
        /// <param name="table">Nome da tabela ou vista a obter os dados</param>
        /// <returns></returns>
        IDtoObject GetRecord(DatabaseTable table, params object[] keys);

        /// <summary>
        /// Devolve uma enumerado com todos os registos da Tabela ou da Vista. Estes dados não são susceptíveis de serem alterados.
        /// </summary>
        /// <param name="table">Nome da tabela ou vista a obter os dados</param>
        /// <returns></returns>
        IEnumerable<IRecordView> GetTable(DatabaseTableOrView table);
        
        /// <summary>
        /// Adiciona um registo da tabela 
        /// </summary>
        /// <param name="table">Tabela a adicionar</param>
        /// <param name="record">Item a adicionar</param>
        void AddRecord(DatabaseTable table, IDtoObject record);

        /// <summary>
        /// Atualiza um registo da tabela 
        /// </summary>
        /// <param name="table">Tabela a atualizar</param>
        /// <param name="record">Item a atualizar</param>
        void UpdateRecord(DatabaseTable table, IDtoObject record);

        /// <summary>
        /// Apaga um registo da tabela 
        /// </summary>
        /// <param name="table">Tabela a apagar</param>
        /// <param name="record">Item a apagar</param>
        void DeleteRecord(DatabaseTable table, IDtoObject record);

        /// <summary>
        /// Disponibiliza uma forma simplificada de adicionar uma bid, sem que seja necessário obter todos os dados das tabelas originais.
        /// </summary>
        /// <param name="saleId">ID da venda</param>
        /// <param name="userName">Nome do utilizador</param>
        /// <param name="value">Valor da licitação</param>
        void AddBid(int saleId, string userName, decimal value);

        /// <summary>
        /// Devolve todas as licitações de determinado leilão. A estrutura devolvida é consistente com o schema fornecido para exportações
        /// podendo ser serializada em XML.
        /// </summary>
        /// <param name="auctionId">ID do leilão</param>
        /// <returns>Licitações do leilão na estrutura para exportação</returns>
        Export.Auction ExportAuctionData(int auctionId);
    }

    public struct Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public enum DatabaseTable
    {
        User, Item, ItemCondition, Sale, Auction, Bid, None
    }

    public enum DatabaseTableOrView
    {
        User, Item, ItemCondition, Sale, Auction, Bid, ActiveAuctionsView
    }
}
