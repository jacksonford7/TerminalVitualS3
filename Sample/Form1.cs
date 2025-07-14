using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aduanas;
using N4;
using N4Ws;
using N4Ws.Entidad;
using Aduana;
using Aduanas.Entidades;
using ControlPagos.Importacion;
using Salesforces;

namespace Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            var RS = N4.Exportacion.ClientePago.ObtenerClientesIndividual("6106785A", Environment.UserName);

            //NO SE PUDO RECUPERAR POR ERROR 
            if (!RS.Exitoso)
            {
                MessageBox.Show(RS.MensajeProblema);
            }
            //SI SE RECUPERO AHORA HAY QUE RECUPERAR NOMBRE DE CLIENTE y sus datos
            var lc = new List<N4.Entidades.Cliente>();
            foreach (var c in RS.Resultado)
            {
               var ci= N4.Entidades.Cliente.ObtenerCliente(Environment.UserName, c.Value);
                if (ci.Exitoso)
                {
                    comboBox1.Items.Add(string.Format("{2}>{1} {0}",ci.Resultado.CLNT_NAME,ci.Resultado.CLNT_CUSTOMER,c.Key));
                }
            }
            */
            //ahora el combo






            var co = new N4.Importacion.container();

            co.CNTR_CONTAINER = "MNBU9093522";
            co.CNTR_CLNT_CUSTOMER_LINE = "MSK";
            co.CNTR_VEPR_REFERENCE = "MSK2020019";
            /*
             *  c.CNTR_VEPR_REFERENCE, c.CNTR_CLNT_CUSTOMER_LINE, c.CNTR_CONTAINER);
             */


            var L = new List<N4.Importacion.container>();
            L.Add(co);
            var r = N4.Importacion.container.ValidacionReeferImpo(L, "gs");
            if (!r.Exitoso)
            {
                MessageBox.Show("Hubo un error de base de datos o codigo");
                return;
            }
            else
            {
                //pregunto si todos los registros o valudaciones estan ok


                //TUPLA: item1=>CNTR ID, item2=>mensaje_horas, item3=>mensaje_otros, item4=>pasa o no pasa (bool)
                var no_pasa = r.Resultado.Where(f => !f.Item4).Count() > 0;
                if(no_pasa)
                {
                    //aqui puedo recuperar todos aquellos que no pasan validacion
                    //si quieres recupera la unidad/es para garbar un log  o enviar un mensaje personalizado
                    var novalidos = r.Resultado.Where(g => !g.Item4).ToList();
                    StringBuilder tb = new StringBuilder();
                    novalidos.ForEach(t=> {
                        tb.AppendFormat("Unidad:{0}->{1} /",t.Item1, !string.IsNullOrEmpty(t.Item3)?t.Item3:t.Item2);
                    });
                    MessageBox.Show(tb.ToString());


                    return;
                }

            }
         



        }
    }
}
