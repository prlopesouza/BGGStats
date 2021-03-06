﻿namespace BGG_PlayStats
{
    partial class FormStats
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbFactions = new System.Windows.Forms.ListBox();
            this.btnAggregate = new System.Windows.Forms.Button();
            this.btnSyncPlays = new System.Windows.Forms.Button();
            this.cbMinPlayers = new System.Windows.Forms.ComboBox();
            this.cbMaxPlayers = new System.Windows.Forms.ComboBox();
            this.dgStats = new System.Windows.Forms.DataGridView();
            this.Faccoes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pVitoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pontuacaoMedia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Partidas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartidasPont = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRemove = new System.Windows.Forms.Button();
            this.cbApenasSelecionados = new System.Windows.Forms.CheckBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.pbLoadBar = new System.Windows.Forms.ProgressBar();
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbAvgPlayTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgStats)).BeginInit();
            this.SuspendLayout();
            // 
            // lbFactions
            // 
            this.lbFactions.FormattingEnabled = true;
            this.lbFactions.Location = new System.Drawing.Point(562, 12);
            this.lbFactions.Name = "lbFactions";
            this.lbFactions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbFactions.Size = new System.Drawing.Size(217, 199);
            this.lbFactions.TabIndex = 0;
            // 
            // btnAggregate
            // 
            this.btnAggregate.Location = new System.Drawing.Point(12, 390);
            this.btnAggregate.Name = "btnAggregate";
            this.btnAggregate.Size = new System.Drawing.Size(75, 23);
            this.btnAggregate.TabIndex = 1;
            this.btnAggregate.Text = "Agregar";
            this.btnAggregate.UseVisualStyleBackColor = true;
            this.btnAggregate.Click += new System.EventHandler(this.btnAggregate_Click);
            // 
            // btnSyncPlays
            // 
            this.btnSyncPlays.Location = new System.Drawing.Point(481, 390);
            this.btnSyncPlays.Name = "btnSyncPlays";
            this.btnSyncPlays.Size = new System.Drawing.Size(75, 23);
            this.btnSyncPlays.TabIndex = 2;
            this.btnSyncPlays.Text = "Sync Plays";
            this.btnSyncPlays.UseVisualStyleBackColor = true;
            this.btnSyncPlays.Click += new System.EventHandler(this.btnSyncPlays_Click);
            // 
            // cbMinPlayers
            // 
            this.cbMinPlayers.FormattingEnabled = true;
            this.cbMinPlayers.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cbMinPlayers.Location = new System.Drawing.Point(617, 392);
            this.cbMinPlayers.Name = "cbMinPlayers";
            this.cbMinPlayers.Size = new System.Drawing.Size(73, 21);
            this.cbMinPlayers.TabIndex = 3;
            // 
            // cbMaxPlayers
            // 
            this.cbMaxPlayers.FormattingEnabled = true;
            this.cbMaxPlayers.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cbMaxPlayers.Location = new System.Drawing.Point(704, 392);
            this.cbMaxPlayers.Name = "cbMaxPlayers";
            this.cbMaxPlayers.Size = new System.Drawing.Size(73, 21);
            this.cbMaxPlayers.TabIndex = 4;
            // 
            // dgStats
            // 
            this.dgStats.AllowUserToAddRows = false;
            this.dgStats.AllowUserToDeleteRows = false;
            this.dgStats.AllowUserToResizeRows = false;
            this.dgStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgStats.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Faccoes,
            this.pVitoria,
            this.pontuacaoMedia,
            this.Partidas,
            this.PartidasPont});
            this.dgStats.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgStats.Location = new System.Drawing.Point(12, 12);
            this.dgStats.Name = "dgStats";
            this.dgStats.ReadOnly = true;
            this.dgStats.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgStats.Size = new System.Drawing.Size(544, 372);
            this.dgStats.TabIndex = 5;
            // 
            // Faccoes
            // 
            this.Faccoes.HeaderText = "Facções";
            this.Faccoes.Name = "Faccoes";
            this.Faccoes.ReadOnly = true;
            // 
            // pVitoria
            // 
            this.pVitoria.HeaderText = "% Vitória";
            this.pVitoria.Name = "pVitoria";
            this.pVitoria.ReadOnly = true;
            // 
            // pontuacaoMedia
            // 
            this.pontuacaoMedia.HeaderText = "Pontuação Média";
            this.pontuacaoMedia.Name = "pontuacaoMedia";
            this.pontuacaoMedia.ReadOnly = true;
            // 
            // Partidas
            // 
            this.Partidas.HeaderText = "Partidas";
            this.Partidas.Name = "Partidas";
            this.Partidas.ReadOnly = true;
            // 
            // PartidasPont
            // 
            this.PartidasPont.HeaderText = "Partidas com Pontuação";
            this.PartidasPont.Name = "PartidasPont";
            this.PartidasPont.ReadOnly = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(93, 390);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.Text = "Remover";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // cbApenasSelecionados
            // 
            this.cbApenasSelecionados.AutoSize = true;
            this.cbApenasSelecionados.Location = new System.Drawing.Point(563, 218);
            this.cbApenasSelecionados.Name = "cbApenasSelecionados";
            this.cbApenasSelecionados.Size = new System.Drawing.Size(127, 17);
            this.cbApenasSelecionados.TabIndex = 7;
            this.cbApenasSelecionados.Text = "Apenas selecionados";
            this.cbApenasSelecionados.UseVisualStyleBackColor = true;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Location = new System.Drawing.Point(704, 214);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(75, 23);
            this.btnFiltrar.TabIndex = 8;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // pbLoadBar
            // 
            this.pbLoadBar.Location = new System.Drawing.Point(12, 419);
            this.pbLoadBar.Name = "pbLoadBar";
            this.pbLoadBar.Size = new System.Drawing.Size(766, 23);
            this.pbLoadBar.Step = 1;
            this.pbLoadBar.TabIndex = 9;
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(563, 242);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(215, 142);
            this.txtDebug.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 457);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Average Play Time:";
            // 
            // lbAvgPlayTime
            // 
            this.lbAvgPlayTime.AutoSize = true;
            this.lbAvgPlayTime.Location = new System.Drawing.Point(118, 457);
            this.lbAvgPlayTime.Name = "lbAvgPlayTime";
            this.lbAvgPlayTime.Size = new System.Drawing.Size(16, 13);
            this.lbAvgPlayTime.TabIndex = 12;
            this.lbAvgPlayTime.Text = "---";
            // 
            // FormStats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 482);
            this.Controls.Add(this.lbAvgPlayTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.pbLoadBar);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.cbApenasSelecionados);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.dgStats);
            this.Controls.Add(this.cbMaxPlayers);
            this.Controls.Add(this.cbMinPlayers);
            this.Controls.Add(this.btnSyncPlays);
            this.Controls.Add(this.btnAggregate);
            this.Controls.Add(this.lbFactions);
            this.Name = "FormStats";
            this.Text = "Form Stats";
            ((System.ComponentModel.ISupportInitialize)(this.dgStats)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbFactions;
        private System.Windows.Forms.Button btnAggregate;
        private System.Windows.Forms.Button btnSyncPlays;
        private System.Windows.Forms.ComboBox cbMinPlayers;
        private System.Windows.Forms.ComboBox cbMaxPlayers;
        private System.Windows.Forms.DataGridView dgStats;
        private System.Windows.Forms.DataGridViewTextBoxColumn Faccoes;
        private System.Windows.Forms.DataGridViewTextBoxColumn pVitoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn pontuacaoMedia;
        private System.Windows.Forms.DataGridViewTextBoxColumn Partidas;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartidasPont;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.CheckBox cbApenasSelecionados;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.ProgressBar pbLoadBar;
        private System.Windows.Forms.TextBox txtDebug;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbAvgPlayTime;
    }
}