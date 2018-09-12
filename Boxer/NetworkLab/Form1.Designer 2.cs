namespace NetworkLab
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.данныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьОбучающиеДанныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьВалидационныеДанныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обработатьОдноИзображениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обработатьПланшетToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьФинансовыеДанныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьПрограммДанныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.нейроннаяСетьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьСтандартнуюМодельToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьИзФайлаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьВФайлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьБольшуюМодельToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.визуализаторToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.финСетьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.входнаяСетьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оптимизаторToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.momentumPaarallelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adamParallelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.предобучениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настроитьПредобучениеСлояToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.предобучатьСлойToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.остановитьПредобучениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обработатьПланшетПреобученнойСетьюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обучениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.начатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.остановитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.openNeuralDialog = new System.Windows.Forms.OpenFileDialog();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.тестСкоростиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Highlight;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.данныеToolStripMenuItem,
            this.нейроннаяСетьToolStripMenuItem,
            this.оптимизаторToolStripMenuItem,
            this.предобучениеToolStripMenuItem,
            this.обучениеToolStripMenuItem,
            this.помощьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(723, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // данныеToolStripMenuItem
            // 
            this.данныеToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.данныеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.загрузитьОбучающиеДанныеToolStripMenuItem,
            this.загрузитьВалидационныеДанныеToolStripMenuItem,
            this.обработатьОдноИзображениеToolStripMenuItem,
            this.обработатьПланшетToolStripMenuItem,
            this.eToolStripMenuItem,
            this.загрузитьФинансовыеДанныеToolStripMenuItem,
            this.загрузитьПрограммДанныеToolStripMenuItem});
            this.данныеToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.данныеToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.данныеToolStripMenuItem.Name = "данныеToolStripMenuItem";
            this.данныеToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.данныеToolStripMenuItem.Text = "Данные";
            this.данныеToolStripMenuItem.Click += new System.EventHandler(this.данныеToolStripMenuItem_Click);
            // 
            // загрузитьОбучающиеДанныеToolStripMenuItem
            // 
            this.загрузитьОбучающиеДанныеToolStripMenuItem.Name = "загрузитьОбучающиеДанныеToolStripMenuItem";
            this.загрузитьОбучающиеДанныеToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.загрузитьОбучающиеДанныеToolStripMenuItem.Text = "Загрузить обучающие данные";
            this.загрузитьОбучающиеДанныеToolStripMenuItem.Click += new System.EventHandler(this.загрузитьОбучающиеДанныеToolStripMenuItem_Click);
            // 
            // загрузитьВалидационныеДанныеToolStripMenuItem
            // 
            this.загрузитьВалидационныеДанныеToolStripMenuItem.Name = "загрузитьВалидационныеДанныеToolStripMenuItem";
            this.загрузитьВалидационныеДанныеToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.загрузитьВалидационныеДанныеToolStripMenuItem.Text = "Загрузить валидационные данные";
            this.загрузитьВалидационныеДанныеToolStripMenuItem.Click += new System.EventHandler(this.загрузитьВалидационныеДанныеToolStripMenuItem_Click);
            // 
            // обработатьОдноИзображениеToolStripMenuItem
            // 
            this.обработатьОдноИзображениеToolStripMenuItem.Name = "обработатьОдноИзображениеToolStripMenuItem";
            this.обработатьОдноИзображениеToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.обработатьОдноИзображениеToolStripMenuItem.Text = "Обработать одно изображение";
            // 
            // обработатьПланшетToolStripMenuItem
            // 
            this.обработатьПланшетToolStripMenuItem.Name = "обработатьПланшетToolStripMenuItem";
            this.обработатьПланшетToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.обработатьПланшетToolStripMenuItem.Text = "Обработать планшет";
            this.обработатьПланшетToolStripMenuItem.Click += new System.EventHandler(this.обработатьПланшетToolStripMenuItem_Click);
            // 
            // eToolStripMenuItem
            // 
            this.eToolStripMenuItem.Name = "eToolStripMenuItem";
            this.eToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.eToolStripMenuItem.Text = "Умная сортировка";
            this.eToolStripMenuItem.Click += new System.EventHandler(this.eToolStripMenuItem_Click);
            // 
            // загрузитьФинансовыеДанныеToolStripMenuItem
            // 
            this.загрузитьФинансовыеДанныеToolStripMenuItem.Name = "загрузитьФинансовыеДанныеToolStripMenuItem";
            this.загрузитьФинансовыеДанныеToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.загрузитьФинансовыеДанныеToolStripMenuItem.Text = "Загрузить финансовые данные";
            this.загрузитьФинансовыеДанныеToolStripMenuItem.Click += new System.EventHandler(this.загрузитьФинансовыеДанныеToolStripMenuItem_Click);
            // 
            // загрузитьПрограммДанныеToolStripMenuItem
            // 
            this.загрузитьПрограммДанныеToolStripMenuItem.Name = "загрузитьПрограммДанныеToolStripMenuItem";
            this.загрузитьПрограммДанныеToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.загрузитьПрограммДанныеToolStripMenuItem.Text = "Загрузить программ данные";
            this.загрузитьПрограммДанныеToolStripMenuItem.Click += new System.EventHandler(this.загрузитьПрограммДанныеToolStripMenuItem_Click);
            // 
            // нейроннаяСетьToolStripMenuItem
            // 
            this.нейроннаяСетьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.загрузитьСтандартнуюМодельToolStripMenuItem,
            this.загрузитьИзФайлаToolStripMenuItem,
            this.сохранитьВФайлToolStripMenuItem,
            this.загрузитьБольшуюМодельToolStripMenuItem,
            this.визуализаторToolStripMenuItem,
            this.финСетьToolStripMenuItem,
            this.входнаяСетьToolStripMenuItem,
            this.тестСкоростиToolStripMenuItem});
            this.нейроннаяСетьToolStripMenuItem.Name = "нейроннаяСетьToolStripMenuItem";
            this.нейроннаяСетьToolStripMenuItem.Size = new System.Drawing.Size(107, 20);
            this.нейроннаяСетьToolStripMenuItem.Text = "Нейронная сеть";
            // 
            // загрузитьСтандартнуюМодельToolStripMenuItem
            // 
            this.загрузитьСтандартнуюМодельToolStripMenuItem.Name = "загрузитьСтандартнуюМодельToolStripMenuItem";
            this.загрузитьСтандартнуюМодельToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.загрузитьСтандартнуюМодельToolStripMenuItem.Text = "Загрузить стандартную модель";
            this.загрузитьСтандартнуюМодельToolStripMenuItem.Click += new System.EventHandler(this.загрузитьСтандартнуюМодельToolStripMenuItem_Click);
            // 
            // загрузитьИзФайлаToolStripMenuItem
            // 
            this.загрузитьИзФайлаToolStripMenuItem.Name = "загрузитьИзФайлаToolStripMenuItem";
            this.загрузитьИзФайлаToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.загрузитьИзФайлаToolStripMenuItem.Text = "Загрузить из файла";
            this.загрузитьИзФайлаToolStripMenuItem.Click += new System.EventHandler(this.загрузитьИзФайлаToolStripMenuItem_Click);
            // 
            // сохранитьВФайлToolStripMenuItem
            // 
            this.сохранитьВФайлToolStripMenuItem.Name = "сохранитьВФайлToolStripMenuItem";
            this.сохранитьВФайлToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.сохранитьВФайлToolStripMenuItem.Text = "Сохранить в файл";
            this.сохранитьВФайлToolStripMenuItem.Click += new System.EventHandler(this.сохранитьВФайлToolStripMenuItem_Click);
            // 
            // загрузитьБольшуюМодельToolStripMenuItem
            // 
            this.загрузитьБольшуюМодельToolStripMenuItem.Name = "загрузитьБольшуюМодельToolStripMenuItem";
            this.загрузитьБольшуюМодельToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.загрузитьБольшуюМодельToolStripMenuItem.Text = "Загрузить большую модель";
            this.загрузитьБольшуюМодельToolStripMenuItem.Click += new System.EventHandler(this.загрузитьБольшуюМодельToolStripMenuItem_Click);
            // 
            // визуализаторToolStripMenuItem
            // 
            this.визуализаторToolStripMenuItem.Name = "визуализаторToolStripMenuItem";
            this.визуализаторToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.визуализаторToolStripMenuItem.Text = "Визуализатор";
            this.визуализаторToolStripMenuItem.Click += new System.EventHandler(this.визуализаторToolStripMenuItem_Click);
            // 
            // финСетьToolStripMenuItem
            // 
            this.финСетьToolStripMenuItem.Name = "финСетьToolStripMenuItem";
            this.финСетьToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.финСетьToolStripMenuItem.Text = "Фин сеть";
            this.финСетьToolStripMenuItem.Click += new System.EventHandler(this.финСетьToolStripMenuItem_Click);
            // 
            // входнаяСетьToolStripMenuItem
            // 
            this.входнаяСетьToolStripMenuItem.Name = "входнаяСетьToolStripMenuItem";
            this.входнаяСетьToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.входнаяСетьToolStripMenuItem.Text = "4-входная сеть";
            this.входнаяСетьToolStripMenuItem.Click += new System.EventHandler(this.входнаяСетьToolStripMenuItem_Click);
            // 
            // оптимизаторToolStripMenuItem
            // 
            this.оптимизаторToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.momentumPaarallelToolStripMenuItem,
            this.adamParallelToolStripMenuItem});
            this.оптимизаторToolStripMenuItem.Name = "оптимизаторToolStripMenuItem";
            this.оптимизаторToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.оптимизаторToolStripMenuItem.Text = "Оптимизатор";
            // 
            // momentumPaarallelToolStripMenuItem
            // 
            this.momentumPaarallelToolStripMenuItem.Name = "momentumPaarallelToolStripMenuItem";
            this.momentumPaarallelToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.momentumPaarallelToolStripMenuItem.Text = "MomentumPaarallel";
            this.momentumPaarallelToolStripMenuItem.Click += new System.EventHandler(this.momentumPaarallelToolStripMenuItem_Click);
            // 
            // adamParallelToolStripMenuItem
            // 
            this.adamParallelToolStripMenuItem.Name = "adamParallelToolStripMenuItem";
            this.adamParallelToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.adamParallelToolStripMenuItem.Text = "AdamParallel";
            this.adamParallelToolStripMenuItem.Click += new System.EventHandler(this.adamParallelToolStripMenuItem_Click);
            // 
            // предобучениеToolStripMenuItem
            // 
            this.предобучениеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настроитьПредобучениеСлояToolStripMenuItem,
            this.предобучатьСлойToolStripMenuItem,
            this.остановитьПредобучениеToolStripMenuItem,
            this.обработатьПланшетПреобученнойСетьюToolStripMenuItem});
            this.предобучениеToolStripMenuItem.Name = "предобучениеToolStripMenuItem";
            this.предобучениеToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.предобучениеToolStripMenuItem.Text = "Предобучение";
            // 
            // настроитьПредобучениеСлояToolStripMenuItem
            // 
            this.настроитьПредобучениеСлояToolStripMenuItem.Name = "настроитьПредобучениеСлояToolStripMenuItem";
            this.настроитьПредобучениеСлояToolStripMenuItem.Size = new System.Drawing.Size(311, 22);
            this.настроитьПредобучениеСлояToolStripMenuItem.Text = "Настроить предобучение слоя";
            this.настроитьПредобучениеСлояToolStripMenuItem.Click += new System.EventHandler(this.настроитьПредобучениеСлояToolStripMenuItem_Click);
            // 
            // предобучатьСлойToolStripMenuItem
            // 
            this.предобучатьСлойToolStripMenuItem.Name = "предобучатьСлойToolStripMenuItem";
            this.предобучатьСлойToolStripMenuItem.Size = new System.Drawing.Size(311, 22);
            this.предобучатьСлойToolStripMenuItem.Text = "Предобучать слой";
            this.предобучатьСлойToolStripMenuItem.Click += new System.EventHandler(this.предобучатьСлойToolStripMenuItem_Click);
            // 
            // остановитьПредобучениеToolStripMenuItem
            // 
            this.остановитьПредобучениеToolStripMenuItem.Name = "остановитьПредобучениеToolStripMenuItem";
            this.остановитьПредобучениеToolStripMenuItem.Size = new System.Drawing.Size(311, 22);
            this.остановитьПредобучениеToolStripMenuItem.Text = "Остановить предобучение";
            this.остановитьПредобучениеToolStripMenuItem.Click += new System.EventHandler(this.остановитьПредобучениеToolStripMenuItem_Click);
            // 
            // обработатьПланшетПреобученнойСетьюToolStripMenuItem
            // 
            this.обработатьПланшетПреобученнойСетьюToolStripMenuItem.Name = "обработатьПланшетПреобученнойСетьюToolStripMenuItem";
            this.обработатьПланшетПреобученнойСетьюToolStripMenuItem.Size = new System.Drawing.Size(311, 22);
            this.обработатьПланшетПреобученнойСетьюToolStripMenuItem.Text = "Обработать планшет преобученной сетью";
            this.обработатьПланшетПреобученнойСетьюToolStripMenuItem.Click += new System.EventHandler(this.обработатьПланшетПреобученнойСетьюToolStripMenuItem_Click);
            // 
            // обучениеToolStripMenuItem
            // 
            this.обучениеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.начатьToolStripMenuItem,
            this.остановитьToolStripMenuItem});
            this.обучениеToolStripMenuItem.Name = "обучениеToolStripMenuItem";
            this.обучениеToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.обучениеToolStripMenuItem.Text = "Обучение";
            // 
            // начатьToolStripMenuItem
            // 
            this.начатьToolStripMenuItem.Name = "начатьToolStripMenuItem";
            this.начатьToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.начатьToolStripMenuItem.Text = "Начать";
            this.начатьToolStripMenuItem.Click += new System.EventHandler(this.начатьToolStripMenuItem_Click);
            // 
            // остановитьToolStripMenuItem
            // 
            this.остановитьToolStripMenuItem.Name = "остановитьToolStripMenuItem";
            this.остановитьToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.остановитьToolStripMenuItem.Text = "Остановить";
            this.остановитьToolStripMenuItem.Click += new System.EventHandler(this.остановитьToolStripMenuItem_Click);
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.помощьToolStripMenuItem.Text = "Помощь";
            // 
            // openImageDialog
            // 
            this.openImageDialog.Title = "Выберите изображение";
            // 
            // openNeuralDialog
            // 
            this.openNeuralDialog.FileName = "openNeuralDialog";
            this.openNeuralDialog.InitialDirectory = "/";
            this.openNeuralDialog.Tag = "*.neural";
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.SystemColors.Info;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(530, 28);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(181, 290);
            this.listBox1.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Tag = "*.neural";
            // 
            // listBox2
            // 
            this.listBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(342, 29);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(181, 290);
            this.listBox2.TabIndex = 3;
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.DimGray;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(13, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(324, 290);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // тестСкоростиToolStripMenuItem
            // 
            this.тестСкоростиToolStripMenuItem.Name = "тестСкоростиToolStripMenuItem";
            this.тестСкоростиToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.тестСкоростиToolStripMenuItem.Text = "Тест скорости";
            this.тестСкоростиToolStripMenuItem.Click += new System.EventHandler(this.тестСкоростиToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(723, 334);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Мозговая лаборатория";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem данныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem нейроннаяСетьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оптимизаторToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem предобучениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обучениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьВалидационныеДанныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обработатьОдноИзображениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьСтандартнуюМодельToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem momentumPaarallelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adamParallelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem начатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьИзФайлаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьВФайлToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openImageDialog;
        private System.Windows.Forms.OpenFileDialog openNeuralDialog;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ToolStripMenuItem остановитьToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem обработатьПланшетToolStripMenuItem;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ToolStripMenuItem настроитьПредобучениеСлояToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem предобучатьСлойToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem остановитьПредобучениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обработатьПланшетПреобученнойСетьюToolStripMenuItem;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolStripMenuItem загрузитьБольшуюМодельToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem визуализаторToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьФинансовыеДанныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem финСетьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьОбучающиеДанныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьПрограммДанныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem входнаяСетьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem тестСкоростиToolStripMenuItem;
    }
}

