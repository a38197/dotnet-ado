using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

namespace BidSoftware.Shared.DTODefinition
{
    public partial class DtoInputBox<T> : Form where T : IDtoObject
    {
        private readonly T instance;
        public T ReturnedObject { get { return instance; } }
        public bool Submited { get; private set; }
        private List<Action> setters = new List<Action>();

        public enum InputType { View, Update, Insert, Delete }

        private InputType inType;

        public DtoInputBox(T instance, InputType inType)
        {
            Submited = false;
            this.instance = instance;
            this.inType = inType;
            InitializeComponent();
        }

        private void DtoInputBox_Load(object sender, EventArgs e)
        {
            setFormCaption(instance);
            fillForm(instance);
            addFormOkCancel();
            setFormApearence();
        }

        private void addFormOkCancel()
        {
            Button ok = new Button();
            ok.Text = "OK";
            ok.Size = new Size(56, 23);
            ok.Location = new Point(lastControl.X, lastControl.Y);
            ok.Click += acceptButton;
            lastControl.X += ok.Width + X_SPACING;

            Button cancel = new Button();
            cancel.Text = "Cancel";
            cancel.Size = new Size(56, 23);
            cancel.Location = new Point(lastControl.X, lastControl.Y);
            cancel.Click += cancelButton;
            lastControl.Y += cancel.Height + Y_SPACING;

            AcceptButton = ok;
            CancelButton = cancel;
            Controls.Add(ok);
            Controls.Add(cancel);
        }

        private const int W_FACTOR = 20, Y_FACTOR = 70;
        private void setFormApearence()
        {
            this.Size = new Size(windowWidth + W_FACTOR, windowHeight + Y_FACTOR);
            MaximizeBox = false;
            MinimizeBox = false;
            MaximumSize = Size;
            MinimumSize = Size;
        }

        private void acceptButton(object sender, EventArgs e)
        {
            foreach (Action ac in setters)
                ac();

            Submited = true;

            Close();
        }

        private void cancelButton(object sender, EventArgs e)
        {
            Submited = false;
        }

        private void setFormCaption(IDtoObject instance)
        {
            this.Text = String.Format("{0} - {1}", instance.ObjectName, inType.ToString());
        }

        
        private void fillForm(IDtoObject dto)
        {
            var dtoType = dto.GetType();
            var dtoFields = from prop in dtoType.GetProperties()
                            select new { Property = prop, Defs = prop.GetCustomAttributes<DtoDefAttribute>(false) };

            foreach (var dtoField in dtoFields)
            {
                if (dtoField.Defs.Count() > 0)
                {
                    var definition = dtoField.Defs.First();
                    Label lbl = getLabel(definition);
                    Control input = getControl(dtoField.Property, definition);
                    this.Controls.Add(lbl);
                    this.Controls.Add(input);
                }
                
            }
            
        }

        private const int LEFT = 5, TOP = 5, X_SPACING = 2, Y_SPACING = 2;
        private int windowWidth = 0, windowHeight = 0;
        private Point lastControl = new Point(LEFT, TOP);

        private Control getControl(PropertyInfo prop, DtoDefAttribute def)
        {
            Control ctr = getControlByType(prop, def);

            ctr.Enabled = (inType == InputType.Update && def.AllowUpdate) || (inType == InputType.Insert && def.AllowInsert) ;
            
            reajustLastControl(ctr, true);
            return ctr;
        }

        private Control getControlByType(PropertyInfo prop, DtoDefAttribute def)
        {
            switch (def.Type)
            {
                case FieldType.String:
                    {
                        TextBox txt = new TextBox();
                        if (def.AllowInsert || def.AllowUpdate)
                            setters.Add(() => prop.SetValue(instance, def.GetNullDefault(txt.Text)));
                        
                        if(inType != InputType.Insert)
                            txt.Text = prop.GetValue(instance).ToString();
                        
                        return txt;
                    }
                
                case FieldType.Integer:
                    {
                        MaskedTextBox mTxt = new MaskedTextBox();
                        if (def.AllowInsert || def.AllowUpdate)
                            setters.Add(() => prop.SetValue(instance, Int32.Parse(def.GetNullDefault(mTxt.Text).ToString())));
                        mTxt.Mask = "0000000000";

                        if (inType != InputType.Insert)
                            mTxt.Text = prop.GetValue(instance).ToString();
                        
                        return mTxt;
                    }
                
                case FieldType.Boolean:
                    {
                        CheckBox chk = new CheckBox();
                        if (def.AllowInsert || def.AllowUpdate)
                            setters.Add(() => prop.SetValue(instance, def.GetNullDefault(chk.Checked)));

                        if (inType != InputType.Insert)
                            chk.Checked = (bool)prop.GetValue(instance);
                        
                        return chk;
                    }

                case FieldType.Date:
                    {
                        DateTimePicker dtp = new DateTimePicker();
                        if (def.AllowInsert || def.AllowUpdate)
                            setters.Add(() => prop.SetValue(instance, def.GetNullDefault(dtp.Value.Date)));

                        if (inType != InputType.Insert)
                            dtp.Value = (DateTime) prop.GetValue(instance);
                        
                        return dtp;
                    }

                case FieldType.Money:
                    {
                        MaskedTextBox mTxt = new MaskedTextBox();
                        if (def.AllowInsert || def.AllowUpdate)
                            setters.Add(() => prop.SetValue(instance, Convert.ToDecimal(def.GetNullDefault(mTxt.Text).ToString())));
                        
                        if (inType != InputType.Insert)
                            mTxt.Text = prop.GetValue(instance).ToString();

                        
                        return mTxt;
                    }

                default:
                    throw new ArgumentException("Non valid definition");
            }
        }

        private Label getLabel(DtoDefAttribute def)
        {
            Label lbl = new Label();
            lbl.Text = def.Label;
            reajustLastControl(lbl, false);
            return lbl;
        }

        private void reajustLastControl(Control ctr, bool newLine)
        {
            ctr.Left = lastControl.X;
            ctr.Top = lastControl.Y;

            if (windowHeight < ctr.Top + ctr.Height)
                windowHeight = ctr.Top + ctr.Height;

            if (windowWidth < ctr.Left + ctr.Width)
                windowWidth = ctr.Left + ctr.Width;

            if (newLine)
            {
                lastControl.X = LEFT;
                lastControl.Y += Y_SPACING + ctr.Height;
            }
            else
            {
                lastControl.X += X_SPACING + ctr.Width;
            }
                
        }
    }
}
