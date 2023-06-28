--Últimas questões respondidas.
select top 500 prova.NOMEPROVA, questao.NUMEROQUESTAO, usu.NOME, case when resp.CERTA = "1" THEN "certa" else "errada" end from RESPOSTASUSUARIOS resposta
inner join RESPOSTASQUESTOES resp on resp.CODIGO = resposta.CODIGORESPOSTA
inner join USUARIOS usu on resposta.CODIGOUSUARIO = usu.ID
inner join QUESTOES questao on questao.CODIGO = resposta.CodigoQuestao
inner join PROVA prova on prova.CODIGO = questao.CODIGOPROVA
order by resposta.CODIGO desc

--Últimas respostas Tabuada Divertirda
select top 500 * from ResultadosTabuadaDivertida order by Codigo desc

--Últimas edições de imagem.
select * from Logger where Descricao = "Fazendo edição de imagem" order by Id desc

--Últimas exceções na API
select * from Logger where Tipo = 2 order by Id desc

--Últimos usuários cadastrados - Questões Aqui
select top 500 login, nome, email, DataNascimento from USUARIOS order by ID desc

--Busca últimas questões cadastradas
select top 500 a.ACAO, a.DATAREGISTRO, u.NOME from ACAOUSUARIO a 
inner join USUARIOS u on a.CODIGOUSUARIO = u.ID
order by a.DATAREGISTRO