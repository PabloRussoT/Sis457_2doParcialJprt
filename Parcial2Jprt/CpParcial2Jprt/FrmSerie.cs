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
        // Variable to store the ID of the selected series
        private int id = 0;
        // Variable to indicate if a new series is being created
        private bool esNuevo = false;

        public FrmSerie()
        {
            InitializeComponent();
        }

        private void FrmSerie_Load(object sender, EventArgs e)
        {
            // On form load, set the initial size and list the series.
            Size = new Size(987, 374); // Set initial form size
            listar(); // Load the list of series
        }

        /// <summary>
        /// Lists the series in the DataGridView based on the search parameter.
        /// </summary>
        private void listar()
        {
            // Clear any previous validation errors
            erpTitulo.Clear();
            erpSinopsis.Clear();
            erpDirector.Clear();
            erpEpisodios.Clear();
            erpFechaEstreno.Clear();

            // Get the search parameter from the text box
            string parametro = txtParametro.Text.Trim();
            // Call the listarPa method from SerieCln to get the series
            List<paSerieListar_Result> lista = SerieCln.listarPa(parametro);
            // Assign the list to the DataGridView
            dgvLista.DataSource = lista;
            dgvLista.Refresh(); // Refresh the DataGridView

            // Hide columns that are not relevant for user display
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;
            dgvLista.Columns["sinopsis"].Visible = false; // Hide sinopsis in the list, it will be visible when editing

            // Set column headers for better readability
            dgvLista.Columns["titulo"].HeaderText = "Título";
            dgvLista.Columns["director"].HeaderText = "Director";
            dgvLista.Columns["episodios"].HeaderText = "Episodios";
            dgvLista.Columns["fechaEstreno"].HeaderText = "Fecha de Estreno";

            // Adjust column widths to fit content
            dgvLista.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // Enable/disable edit and delete buttons based on whether there are items in the list
            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;

            // If there are items, select the first row
            if (lista.Count > 0)
            {
                dgvLista.CurrentCell = dgvLista.Rows[0].Cells["titulo"];
            }
        }

        /// <summary>
        /// Clears all input fields in the data entry section.
        /// </summary>
        private void limpiar()
        {
            txtTitulo.Text = string.Empty;
            txtSinopsis.Text = string.Empty;
            txtDirector.Text = string.Empty;
            nudEpisodios.Value = 0;
            dtpFechaEstreno.Value = DateTime.Now; // Set to current date
            id = 0; // Reset the selected series ID
        }

        /// <summary>
        /// Validates the input fields in the data entry section.
        /// </summary>
        /// <returns>True if all fields are valid, false otherwise.</returns>
        private bool validar()
        {
            bool esValido = true;
            // Clear previous errors
            erpTitulo.Clear();
            erpSinopsis.Clear();
            erpDirector.Clear();
            erpEpisodios.Clear();
            erpFechaEstreno.Clear();

            // Validar campos
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

            return esValido;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true; // Indicate that a new series is being created
            Size = new Size(987, 628); // Expand the form to show the data entry section
            limpiar(); // Clear the fields
            txtTitulo.Focus(); // Set focus to the title field
            // Disable action panel and enable data group box
            pnlAcciones.Enabled = false;
            gbxDatos.Enabled = true;
            btnBuscar.Enabled = false;
            txtParametro.Enabled = false;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Check if a row is selected in the DataGridView
            if (dgvLista.Rows.Count > 0 && dgvLista.CurrentRow != null)
            {
                esNuevo = false; // Indicate that an existing series is being edited
                Size = new Size(987, 628); // Expand the form to show the data entry section

                // Get the ID of the series from the selected row
                id = Convert.ToInt32(dgvLista.CurrentRow.Cells["id"].Value);
                // Get the complete series using the ID
                Serie serie = SerieCln.obtenerUno(id);

                // Load the series data into the input fields
                txtTitulo.Text = serie.titulo;
                txtSinopsis.Text = serie.sinopsis;
                txtDirector.Text = serie.director;
                nudEpisodios.Value = serie.episodios;
                dtpFechaEstreno.Value = serie.fechaEstreno;
                txtTitulo.Focus(); // Set focus to the title field

                // Disable action panel and enable data group box
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
                // Create a Serie object with the form data
                Serie serie = new Serie();
                serie.titulo = txtTitulo.Text.Trim();
                serie.sinopsis = txtSinopsis.Text.Trim();
                serie.director = txtDirector.Text.Trim();
                serie.episodios = (int)nudEpisodios.Value;
                serie.fechaEstreno = dtpFechaEstreno.Value;
                serie.estado = 1; // By default, the status is active

                try
                {
                    if (esNuevo)
                    {
                        // Insert new series
                        SerieCln.insertar(serie);
                    }
                    else
                    {
                        // Update existing series
                        serie.id = id; // Assign the ID of the series being edited
                        SerieCln.actualizar(serie);
                    }
                    MessageBox.Show("Serie guardada correctamente", "::: Parcial2Jprt - Mensaje :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listar(); // Refresh the list of series
                    btnCancelar.PerformClick(); // Simulate click on Cancel to clear and reset the state
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar la serie: " + ex.Message, "::: Parcial2Jprt - Error :::", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar(); // Clear the fields
            Size = new Size(987, 374); // Revert to the original form size (collapsed view)
            // Enable action panel and disable data group box
            pnlAcciones.Enabled = true;
            gbxDatos.Enabled = false;
            btnBuscar.Enabled = true;
            txtParametro.Enabled = true;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Check if a row is selected in the DataGridView
            if (dgvLista.Rows.Count > 0 && dgvLista.CurrentRow != null)
            {
                // Get the ID of the series from the selected row
                id = Convert.ToInt32(dgvLista.CurrentRow.Cells["id"].Value);
                // Get the title of the series for the confirmation message
                string tituloSerie = dgvLista.CurrentRow.Cells["titulo"].Value.ToString();

                // Ask for user confirmation before deleting
                DialogResult rpta = MessageBox.Show($"¿Está seguro de eliminar la serie '{tituloSerie}'?",
                                                    "::: Parcial2Jprt - Confirmación :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rpta == DialogResult.Yes)
                {
                    try
                    {
                        SerieCln.eliminar(id); // Call the elimination method
                        MessageBox.Show("Serie eliminada correctamente", "::: Parcial2Jprt - Mensaje :::", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        listar(); // Refresh the list
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
            listar(); // Call the listar method to apply the search filter
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If Enter key is pressed in the parameter field, perform the search
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Suppress the "ding" sound
                listar();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close(); // Close the form
        }

        // Event for when a cell in the DataGridView is clicked
        private void dgvLista_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure a valid row was clicked (not the header)
            if (e.RowIndex >= 0)
            {
                // Store the ID of the series from the selected row
                id = Convert.ToInt32(dgvLista.Rows[e.RowIndex].Cells["id"].Value);
            }
        }
    }
}
