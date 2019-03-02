<Query Kind="Program" />

void Main()
{
  var osc = _context.vwOrdemServicoPorCliente
      .Where(w => w.PrazoEntrega >= starDate && w.PrazoEntrega <= endDate)
      .GroupBy(gb => new
      {
          gb.CodigoCliente,
          gb.NomeCliente,
          gb.PrazoEntrega
      })
      .Select(s => new
      {
          s.Key.CodigoCliente,
          s.Key.NomeCliente,
          s.Key.PrazoEntrega,
          QtdeObjetos = s.Sum(x => x.QtdeObjetos),
          QtdeImpressao = s.Sum(x => x.QtdeObjetos) - s.Sum(x => x.Impressao),
          QtdeAcabamento = s.Sum(x => x.QtdeObjetos) - s.Sum(x => x.Acabamento),
          QtdePostagem = s.Sum(x => x.QtdeObjetos) - s.Sum(x => x.Postagem)
      }).OrderBy(o => o.NomeCliente).ToList();

  var monitorList = osc
      .Select(s => new
      {
          ClienteId = s.CodigoCliente, 
          ClienteName = s.NomeCliente
      })
      .Distinct()
          .Select(item => new Monitor 
          {
              CodigoCliente = item.ClienteId, 
              NomeCliente = item.ClienteName,
              OrdemServicoList = osc
                  .Where(w => w.CodigoCliente == item.ClienteId)
                  .Select(s => new OrdemServico
                  {
                      PrazoEntrega = s.PrazoEntrega, 
                      QtdeObjetos = s.QtdeObjetos, 
                      QtdeImpressao = s.QtdeImpressao, 
                      QtdeAcabamento = s.QtdeAcabamento, 
                      QtdePostagem = s.QtdePostagem
                  }).ToList()
          });

}
