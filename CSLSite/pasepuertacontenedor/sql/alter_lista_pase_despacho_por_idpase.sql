ALTER PROCEDURE [dbo].[lista_pase_despacho_por_idpase]
    @id_pase BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pd.CONTAINER,
        pd.MRN,
        pd.MSN,
        pd.HSN,
        pd.BL,
        pd.ITEM,
        pd.SELLO1,
        pd.SELLO2,
        pd.ISO,
        pd.NETO,
        pd.BRUTO,
        pd.LINE,
        pd.RUC,
        pd.EMPRESA,
        pd.PROVINCIA,
        pd.PLACA,
        pd.LICENCIA,
        pd.CONDUCTOR,
        pd.TELEFONO,
        pd.PASE,
        pd.TTURNO,
        pd.TINICIO,
        pd.TFIN,
        pd.DOCUMENTO,
        pd.UBICACION,
        pd.SELLO3,
        pd.SELLO4,
        pd.SELLOCGSA,
        pd.BUQUE,
        pd.IMPORTADOR,
        pd.ADUANA,
        pd.SN,
        pd.CERTIFICADO_CODBARRA
    FROM dbo.PASE_DESPACHO pd
    WHERE pd.ID_PASE = @id_pase;
END

