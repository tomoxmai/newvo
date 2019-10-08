﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace ok
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con;
        DataTable dt;
        public MainWindow()
        {
            InitializeComponent();
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\AlummnosDB.mdb";
            MostrarDatos();
        }
        //mostramos los registros de la tabla
        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Progra";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();
            if (dt.Rows.Count > 0)
            {
                IbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                IbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        private void  LimpiarTodo()
        {
            txtId.Text = "";
            txtnombre.Text = "";
            cbGenero.SelectedIndex = 0;
            txtTelefono.Text = "";
            txtDireccion.Text = "";
            btnNuevo.Content = "Nuevo";
            txtId.IsEnabled = true;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            if(txtId.Text!="")
            {
                if(cbGenero.Text!="Selecciona Genero")
                {
                    cmd.CommandText = "insert into Progra(Id,Nombre,Genero,Telefono,Direccion)" + "Values(" + txtId.Text + ",'" + txtnombre.Text + ",'" + cbGenero.Text + ",'" + txtTelefono.Text + ",'" + txtDireccion.Text + ")'";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Alumno agregado correctamente...");
                    LimpiarTodo();
                }
                else
                {
                    MessageBox.Show("Favor de seleccionar el genero...");
                }
            }
            else
            {
                cmd.CommandText = "update Progra set Nombre='" + txtnombre.Text + "',Genero='" + cbGenero.Text + "',Telefono" + txtTelefono.Text + ",'Direccion='" + txtDireccion.Text + "'where Id=" + txtId.Text;
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Datos del alumno Actualizados...");
                LimpiarTodo();


            }

        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if(gvDatos.SelectedItems.Count>0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtId.Text = row["Id"].ToString();
                txtnombre.Text = row["Nombre"].ToString();
                cbGenero.Text = row["Genero"].ToString();
                txtTelefono.Text = row["Telefono"].ToString();
                txtDireccion.Text = row["Direccion"].ToString();
                txtId.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de Seleccionar un alumno");
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from progra where Id=" + row["Id"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Alumno eliminado correctamente...");
                LimpiarTodo();
            }
            else
            {
                MessageBox.Show("Favor de Seleccionar un alumno");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarTodo();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
