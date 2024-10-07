namespace TrackerUI
{
    partial class CreateTeamForm
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
            headerLabel = new Label();
            teamNameValue = new TextBox();
            TeamNameLabel = new Label();
            addMemberButton = new Button();
            selectTeamMemberDropDown = new ComboBox();
            selectTeamMemberLabel = new Label();
            addNewmemberGroupBox = new GroupBox();
            firstNameValue = new TextBox();
            firstNameLabel = new Label();
            lastNameValue = new TextBox();
            lastNameLabel = new Label();
            emailValue = new TextBox();
            emailLabel = new Label();
            phoneNumberValue = new TextBox();
            phoneNumber = new Label();
            createMemberButton = new Button();
            teamMembersListBox = new ListBox();
            deletedSelectedMemberButton = new Button();
            CreateTeamButton = new Button();
            addNewmemberGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // headerLabel
            // 
            headerLabel.AutoSize = true;
            headerLabel.Font = new Font("Segoe UI Light", 28.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            headerLabel.ForeColor = SystemColors.MenuHighlight;
            headerLabel.Location = new Point(25, 45);
            headerLabel.Name = "headerLabel";
            headerLabel.Size = new Size(272, 62);
            headerLabel.TabIndex = 2;
            headerLabel.Text = "Create Team";
            // 
            // teamNameValue
            // 
            teamNameValue.Location = new Point(43, 201);
            teamNameValue.Name = "teamNameValue";
            teamNameValue.Size = new Size(540, 43);
            teamNameValue.TabIndex = 13;
            //teamNameValue.TextChanged += this.teamNameValue_TextChanged;
            // 
            // TeamNameLabel
            // 
            TeamNameLabel.AutoSize = true;
            TeamNameLabel.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TeamNameLabel.ForeColor = SystemColors.MenuHighlight;
            TeamNameLabel.Location = new Point(43, 152);
            TeamNameLabel.Name = "TeamNameLabel";
            TeamNameLabel.Size = new Size(197, 46);
            TeamNameLabel.TabIndex = 12;
            TeamNameLabel.Text = "Team Name";
            //TeamNameLabel.Click += TeamNameLabel_Click;
            // 
            // addMemberButton
            // 
            addMemberButton.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            addMemberButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(102, 102, 102);
            addMemberButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 242, 242);
            addMemberButton.FlatStyle = FlatStyle.Flat;
            addMemberButton.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            addMemberButton.ForeColor = SystemColors.MenuHighlight;
            addMemberButton.Location = new Point(181, 393);
            addMemberButton.Name = "addMemberButton";
            addMemberButton.Size = new Size(242, 78);
            addMemberButton.TabIndex = 19;
            addMemberButton.Text = "Add Member";
            addMemberButton.UseVisualStyleBackColor = true;
            // 
            // selectTeamMemberDropDown
            // 
            selectTeamMemberDropDown.FormattingEnabled = true;
            selectTeamMemberDropDown.Location = new Point(43, 318);
            selectTeamMemberDropDown.Name = "selectTeamMemberDropDown";
            selectTeamMemberDropDown.Size = new Size(540, 45);
            selectTeamMemberDropDown.TabIndex = 18;
            selectTeamMemberDropDown.SelectedIndexChanged += selectTeamMemberDropDown_SelectedIndexChanged;
            // 
            // selectTeamMemberLabel
            // 
            selectTeamMemberLabel.AutoSize = true;
            selectTeamMemberLabel.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            selectTeamMemberLabel.ForeColor = SystemColors.MenuHighlight;
            selectTeamMemberLabel.Location = new Point(43, 269);
            selectTeamMemberLabel.Name = "selectTeamMemberLabel";
            selectTeamMemberLabel.Size = new Size(335, 46);
            selectTeamMemberLabel.TabIndex = 17;
            selectTeamMemberLabel.Text = "Select Team Member";
            // 
            // addNewmemberGroupBox
            // 
            addNewmemberGroupBox.Controls.Add(createMemberButton);
            addNewmemberGroupBox.Controls.Add(phoneNumberValue);
            addNewmemberGroupBox.Controls.Add(phoneNumber);
            addNewmemberGroupBox.Controls.Add(emailValue);
            addNewmemberGroupBox.Controls.Add(emailLabel);
            addNewmemberGroupBox.Controls.Add(lastNameValue);
            addNewmemberGroupBox.Controls.Add(lastNameLabel);
            addNewmemberGroupBox.Controls.Add(firstNameValue);
            addNewmemberGroupBox.Controls.Add(firstNameLabel);
            addNewmemberGroupBox.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            addNewmemberGroupBox.ForeColor = SystemColors.MenuHighlight;
            addNewmemberGroupBox.Location = new Point(43, 504);
            addNewmemberGroupBox.Name = "addNewmemberGroupBox";
            addNewmemberGroupBox.Size = new Size(538, 473);
            addNewmemberGroupBox.TabIndex = 20;
            addNewmemberGroupBox.TabStop = false;
            addNewmemberGroupBox.Text = "Add New Member";
            // 
            // firstNameValue
            // 
            firstNameValue.Location = new Point(225, 78);
            firstNameValue.Name = "firstNameValue";
            firstNameValue.Size = new Size(285, 51);
            firstNameValue.TabIndex = 10;
            // 
            // firstNameLabel
            // 
            firstNameLabel.AutoSize = true;
            firstNameLabel.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            firstNameLabel.ForeColor = SystemColors.MenuHighlight;
            firstNameLabel.Location = new Point(38, 75);
            firstNameLabel.Name = "firstNameLabel";
            firstNameLabel.Size = new Size(181, 46);
            firstNameLabel.TabIndex = 9;
            firstNameLabel.Text = "First Name";
            // 
            // lastNameValue
            // 
            lastNameValue.Location = new Point(225, 158);
            lastNameValue.Name = "lastNameValue";
            lastNameValue.Size = new Size(285, 51);
            lastNameValue.TabIndex = 12;
            // 
            // lastNameLabel
            // 
            lastNameLabel.AutoSize = true;
            lastNameLabel.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lastNameLabel.ForeColor = SystemColors.MenuHighlight;
            lastNameLabel.Location = new Point(38, 155);
            lastNameLabel.Name = "lastNameLabel";
            lastNameLabel.Size = new Size(177, 46);
            lastNameLabel.TabIndex = 11;
            lastNameLabel.Text = "Last Name";
            // 
            // emailValue
            // 
            emailValue.Location = new Point(225, 229);
            emailValue.Name = "emailValue";
            emailValue.Size = new Size(285, 51);
            emailValue.TabIndex = 14;
            // 
            // emailLabel
            // 
            emailLabel.AutoSize = true;
            emailLabel.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            emailLabel.ForeColor = SystemColors.MenuHighlight;
            emailLabel.Location = new Point(38, 226);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new Size(99, 46);
            emailLabel.TabIndex = 13;
            emailLabel.Text = "Email";
            // 
            // phoneNumberValue
            // 
            phoneNumberValue.Location = new Point(221, 295);
            phoneNumberValue.Name = "phoneNumberValue";
            phoneNumberValue.Size = new Size(285, 51);
            phoneNumberValue.TabIndex = 16;
            // 
            // phoneNumber
            // 
            phoneNumber.AutoSize = true;
            phoneNumber.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            phoneNumber.ForeColor = SystemColors.MenuHighlight;
            phoneNumber.Location = new Point(34, 292);
            phoneNumber.Name = "phoneNumber";
            phoneNumber.Size = new Size(176, 46);
            phoneNumber.TabIndex = 15;
            phoneNumber.Text = "Phone No.";
            // 
            // createMemberButton
            // 
            createMemberButton.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            createMemberButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(102, 102, 102);
            createMemberButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 242, 242);
            createMemberButton.FlatStyle = FlatStyle.Flat;
            createMemberButton.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            createMemberButton.ForeColor = SystemColors.MenuHighlight;
            createMemberButton.Location = new Point(121, 380);
            createMemberButton.Name = "createMemberButton";
            createMemberButton.Size = new Size(242, 59);
            createMemberButton.TabIndex = 20;
            createMemberButton.Text = "Create Member";
            createMemberButton.UseVisualStyleBackColor = true;
            // 
            // teamMembersListBox
            // 
            teamMembersListBox.BorderStyle = BorderStyle.FixedSingle;
            teamMembersListBox.FormattingEnabled = true;
            teamMembersListBox.ItemHeight = 37;
            teamMembersListBox.Location = new Point(688, 201);
            teamMembersListBox.Name = "teamMembersListBox";
            teamMembersListBox.Size = new Size(434, 779);
            teamMembersListBox.TabIndex = 21;
            // 
            // deletedSelectedMemberButton
            // 
            deletedSelectedMemberButton.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            deletedSelectedMemberButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(102, 102, 102);
            deletedSelectedMemberButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 242, 242);
            deletedSelectedMemberButton.FlatStyle = FlatStyle.Flat;
            deletedSelectedMemberButton.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            deletedSelectedMemberButton.ForeColor = SystemColors.MenuHighlight;
            deletedSelectedMemberButton.Location = new Point(1164, 536);
            deletedSelectedMemberButton.Name = "deletedSelectedMemberButton";
            deletedSelectedMemberButton.Size = new Size(158, 89);
            deletedSelectedMemberButton.TabIndex = 22;
            deletedSelectedMemberButton.Text = "Delete Selected";
            deletedSelectedMemberButton.UseVisualStyleBackColor = true;
            // 
            // CreateTeamButton
            // 
            CreateTeamButton.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            CreateTeamButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(102, 102, 102);
            CreateTeamButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 242, 242);
            CreateTeamButton.FlatStyle = FlatStyle.Flat;
            CreateTeamButton.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CreateTeamButton.ForeColor = SystemColors.MenuHighlight;
            CreateTeamButton.Location = new Point(518, 1031);
            CreateTeamButton.Name = "CreateTeamButton";
            CreateTeamButton.Size = new Size(298, 86);
            CreateTeamButton.TabIndex = 25;
            CreateTeamButton.Text = "Create Team";
            CreateTeamButton.UseVisualStyleBackColor = true;
            // 
            // CreateTeamForm
            // 
            AutoScaleDimensions = new SizeF(15F, 37F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1345, 1143);
            Controls.Add(CreateTeamButton);
            Controls.Add(deletedSelectedMemberButton);
            Controls.Add(teamMembersListBox);
            Controls.Add(addNewmemberGroupBox);
            Controls.Add(addMemberButton);
            Controls.Add(selectTeamMemberDropDown);
            Controls.Add(selectTeamMemberLabel);
            Controls.Add(teamNameValue);
            Controls.Add(TeamNameLabel);
            Controls.Add(headerLabel);
            Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(6, 6, 6, 6);
            Name = "CreateTeamForm";
            Text = "Create Team";
            //Load += this.CreateTeamForm_Load;
            addNewmemberGroupBox.ResumeLayout(false);
            addNewmemberGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label headerLabel;
        private TextBox teamNameValue;
        private Label TeamNameLabel;
        private Button addMemberButton;
        private ComboBox selectTeamMemberDropDown;
        private Label selectTeamMemberLabel;
        private GroupBox addNewmemberGroupBox;
        private Button createMemberButton;
        private TextBox phoneNumberValue;
        private Label phoneNumber;
        private TextBox emailValue;
        private Label emailLabel;
        private TextBox lastNameValue;
        private Label lastNameLabel;
        private TextBox firstNameValue;
        private Label firstNameLabel;
        private ListBox teamMembersListBox;
        private Button deletedSelectedMemberButton;
        private Button CreateTeamButton;
    }
}