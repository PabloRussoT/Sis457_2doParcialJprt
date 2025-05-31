using ClnParcial2Jprt;
using CadParcial2Jprt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpParcial2Jprt
{
    public partial class FrmSerie : Form
    {
       
        private int id = 0;
 
        private bool esNuevo = false;

        public FrmSerie()
        {
            InitializeComponent();
        }

        private void FrmSerie_Load(object sender, EventArgs e)
        {
          
            Size = new Size(1010, 374); 
            listar(); 
        }

        
        private void listar()
        {
           
            erpTitulo.Clear();
            erpSinopsis.Clear();
            erpDirector.Clear();
            erpEpisodios.Clear();
            erpFechaEstreno.Clear();
            erpUrlTrailer.Clear();
            erpIdiomaOriginal.Clear();


            string parametro = txtParametro.Text.Trim();
          
            List<paSerieListar_Result> lista = SerieCln.listarPa(parametro);
           
            dgvLista.DataSource = lista;
            dgvLista.Refresh(); 

           
            dgvLista.Columns["id"].HeaderText = "Id";
            dgvLista.Columns["estado"].Visible = false;
            dgvLista.Columns["sinopsis"].HeaderText = "Sinopsis"; 

          
            dgvLista.Columns["titulo"].HeaderText = "Título";
            dgvLista.Columns["director"].HeaderText = "Director";
            dgvLista.Columns["episodios"].HeaderText = "Episodios";
            dgvLista.Columns["fechaEstreno"].HeaderText = "Fecha de Estreno";
            dgvLista.Columns["urlTrailer"].HeaderText = "URL del Trailer";
            dgvLista.Columns["idiomaOriginal"].HeaderText = "Idioma Original";


            dgvLista.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

           
            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;

           
            if (lista.Count > 0)
            {
                dgvLista.CurrentCell = dgvLista.Rows[0].Cells["titulo"];
            }
        }

       
        private void limpiar()
        {
            txtTitulo.Text = string.Empty;
            txtSinopsis.Text = string.Empty;
            txtDirector.Text = string.Empty;
            nudEpisodios.Value = 0;
            dtpFechaEstreno.Value = DateTime.Now; 
            id = 0; 
        }

      
        private bool validar()
        {
            bool esValido = true;
           
            erpTitulo.Clear();
            erpSinopsis.Clear();
            erpDirector.Clear();
            erpEpisodios.Clear();
            erpFechaEstreno.Clear();
            erpUrlTrailer.Clear();
            erpIdiomaOriginal.Clear();


            if (string.IsNullOrEmpty(txtTitulo.Text))
            {
                erpTitulo.SetError(txtTitulo, "El título es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtSinopsis.Text))
            {
                erpSinopsis.SetError(txtSinopsis, "La sinopsis es obligatoria");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtDirector.Text))
            {
                erpDirector.SetError(txtDirector, "El director es obligatorio");
                esValido = false;
            }
            if (nudEpisodios.Value == 0)
            {
                erpEpisodios.SetError(nudEpisodios, "El número de episodios debe ser mayor a 0");
                esValido = false;
            }
            if (dtpFechaEstreno.Value > DateTime.Now)
            {
                erpFechaEstreno.SetError(dtpFechaEstreno, "La fecha de estreno no puede ser futura");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtTitulo.Text))
            {
                erpUrlTrailer.SetError(txtUrlTrailer, "El Trailer es obligatorio");
                esValido = false;
            }

            return esValido;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true; 
            Size = new Size(987, 628);
            limpiar(); 
            txtTitulo.Focus(); 
           
            pnlAcciones.Enabled = false;
            gbxDatos.Enabled = true;
            btnBuscar.Enabled = false;
            txtParametro.Enabled = false;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
           
            if (dgvLista.Rows.Count > 0 && dgvLista.CurrentRow != null)
            {
                esNuevo = false; 
                Size = new Size(1000, 628); 

               
                id = Convert.ToInt32(dgvLista.CurrentRow.Cells["id"].Value);
            
                Serie serie = SerieCln.obtenerUno(id);

            
                txtTitulo.Text = serie.titulo;
                txtSinopsis.Text = serie.sinopsis;
                txtDirector.Text = serie.director;
                nudEpisodios.Value = serie.episodios;
                dtpFechaEstreno.Value = serie.fechaEstreno;
                txtUrlTrailer.Text = serie.urlTrailer;
                txtIdiomaOriginal.Text = serie.idiomaOriginal;
                txtTitulo.Focus();

                
                pnlAcciones.Enabled = false;
                gbxDatos.Enabled = true;
                btnBuscar.Enabled = false;
                txtParametro.Enabled = false;
            }
            else
            {
                MessageBox.Show("Seleccione una fila para editar", "::: Parcial2Jprt - Advertencia :::", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                
                Serie serie = new Serie();
                serie.titulo = txtTitulo.Text.Trim();
                serie.sinopsis = txtSinopsis.Text.Trim();
                serie.director = txtDirector.Text.Trim();
                serie.episodios = (int)nudEpisodios.Value;
                serie.fechaEstreno = dtpFechaEstreno.Value;
                serie.estado = 1;
                serie.urlTrailer = txtUrlTrailer.Text.Trim();
                serie.idiomaOriginal = txtIdiomaOriginal.Text.Trim();


                try
                {
                    if (esNuevo)
                    {
                       
                        SerieCln.insertar(serie);
                    }
                    else
                    {
                       
                        serie.id = id; 
                        SerieCln.actualizar(serie);
                    }
                    MessageBox.Show("Serie guardada correctamente", "::: Parcial2Jprt - Mensaje :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listar();
                    btnCancelar.PerformClick(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar la serie: " + ex.Message, "::: Parcial2Jprt - Error :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar(); 
            Size = new Size(1000, 374);
          
            pnlAcciones.Enabled = true;
            gbxDatos.Enabled = false;
            btnBuscar.Enabled = true;
            txtParametro.Enabled = true;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
           
            if (dgvLista.Rows.Count > 0 && dgvLista.CurrentRow != null)
            {
               
                id = Convert.ToInt32(dgvLista.CurrentRow.Cells["id"].Value);
                
                string tituloSerie = dgvLista.CurrentRow.Cells["titulo"].Value.ToString();

               
                DialogResult rpta = MessageBox.Show($"¿Está seguro de eliminar la serie '{tituloSerie}'?",
                                                    "::: Parcial2Jprt - Confirmación :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rpta == DialogResult.Yes)
                {
                    try
                    {
                        SerieCln.eliminar(id);
                        MessageBox.Show("Serie eliminada correctamente", "::: Parcial2Jprt - Mensaje :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        listar(); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar la serie: " + ex.Message, "::: Parcial2Jprt - Error :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una fila para eliminar", "::: Parcial2Jprt - Advertencia :::", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; 
                listar();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close(); 
        }

        
        private void dgvLista_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (e.RowIndex >= 0)
            {
               
                id = Convert.ToInt32(dgvLista.Rows[e.RowIndex].Cells["id"].Value);
            }
        }
    }
}
