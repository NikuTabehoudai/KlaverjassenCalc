using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using KlaverjassenCalc.Model;
using KlaverjassenCalc.ViewModel;

namespace KlaverjassenCalc
{
	class DataList : BaseViewModel
	{

		public string Roemchoice { get; set; }
		public int Roem { get; set; }

		public DataList()
		{
			InputCommand = new Command(InputButton);
			OpenEditCommand = new Command(OpenEditButton);
			CloseRoundChoiceCommand = new Command(CloseRoundChoiceButton);
			EditRoundChoiceCommand = new Command(EditRoundChoiceButton);
			CloseEditCommand = new Command(CloseEditButton);
			EditCommand = new Command(EditButton);
			ResetCommand = new Command(ResetButton);
			CloseCommand = new Command(CloseButton);
			OptionsCommand = new Command(OptionsButton);
			RoemZijCommand = new Command(RoemZijButton);
			RoemWijCommand = new Command(RoemWijButton);
			OneRoemCommand = new Command(OneRoemButton);
			TwoRoemCommand = new Command(TwoRoemButton);
			ThreeRoemCommand = new Command(ThreeRoemButton);
			FourRoemCommand = new Command(FourRoemButton);
			RoemCloseCommand = new Command(RoemCloseButton);
			InputWindowCommand = new Command(InputWindowButton);
			CloseInputCommand = new Command(CloseInputButton);
			ResetInputCommand = new Command(ResetInputButton);
			CloseResultCommand = new Command(CloseResultButton);

			LoadXml();



		}

		public ObservableCollection<InputDataList> _InputDataList = new ObservableCollection<InputDataList>();
		public ObservableCollection<InputDataList> InputDataList
		{
			get { return _InputDataList; }
			set
			{
				_InputDataList = value;
				OnPropertyChanged(nameof(InputDataList));
			}
		}

		//Buttons
		#region Buttons
		public Command InputCommand { get; set; }

		public void InputButton()
		{

			var list = new List<string>();
			if (RondeBind != "Klaar")
			{
				if (!string.IsNullOrWhiteSpace(RondeBind))
				{
					if ((string.IsNullOrWhiteSpace(PuntenWijBind) || string.IsNullOrWhiteSpace(PuntenZijBind)) && (!string.IsNullOrWhiteSpace(PuntenWijBind) || !string.IsNullOrWhiteSpace(PuntenZijBind)))
					{

						list = Calc(PuntenWijInput, PuntenZijInput, Int16.Parse(RoemWijInput), Int16.Parse(RoemZijInput));
							if (list.Count != 0)
								if (list[0].ToLower() == "true")
								{
									Input(RondeInput, list[1], Int16.Parse(list[2]), list[3], Int16.Parse(list[4]), Int16.Parse(list[5]), Int16.Parse(list[6]));
									InputWindowVisible = "false";
								}
                    }
				}
			}

			if (InputDataList.Count == 16)
            {
				RondeBind = "Klaar";
				ResultWindowVisible = "true";
			}

		}


		public Command OpenEditCommand { get; set; }

		public void OpenEditButton()
		{
			RoundSelectVisible = "true";
			EditWindowVisible = "false";
		}

		public Command CloseRoundChoiceCommand { get; set; }

		public void CloseRoundChoiceButton()
		{
			RoundSelectVisible = "false";
			RondeInputEdit = null;
		}

		public Command EditRoundChoiceCommand { get; set; }

		public void EditRoundChoiceButton()
		{

			if (!String.IsNullOrWhiteSpace(RondeInputEdit) && InputDataList.Count != 0)
				if (RondeInputEdit.All(char.IsDigit))
					if (Int16.Parse(RondeInputEdit) != 0)
						if (InputDataList.Count() >= Int16.Parse(RondeInputEdit))
						{
							EditWindowVisible = "true";
							RoundSelectVisible = "false";

							RondeEdit = RondeInputEdit;
							PuntenWijPlaceHolderEdit = InputDataList[Int16.Parse(RondeInputEdit) - 1].PuntenWij;
							PuntenZijPlaceHolderEdit = InputDataList[Int16.Parse(RondeInputEdit) - 1].PuntenZij;
							RoemWijPlaceHolderEdit = InputDataList[Int16.Parse(RondeInputEdit) - 1].RoemWij.ToString();
							RoemZijPlaceHolderEdit = InputDataList[Int16.Parse(RondeInputEdit) - 1].RoemZij.ToString();


						}
			RondeInputEdit = null;
		}

		public Command EditCommand { get; set; }

		public void EditButton()
		{
			List<string> list;

			if (string.IsNullOrWhiteSpace(RoemWijEdit))
				RoemWijEdit = RoemWijPlaceHolderEdit;

			if (string.IsNullOrWhiteSpace(RoemZijEdit))
				RoemZijEdit = RoemZijPlaceHolderEdit;

			if ((string.IsNullOrWhiteSpace(PuntenWijEdit) || string.IsNullOrWhiteSpace(PuntenZijEdit)) && (!string.IsNullOrWhiteSpace(PuntenWijEdit) || !string.IsNullOrWhiteSpace(PuntenZijEdit)))
			{
				list = Fix(Int16.Parse(RondeEdit), PuntenWijEdit, PuntenZijEdit, Int16.Parse(RoemWijEdit), Int16.Parse(RoemZijEdit));
				if (list.Count != 0)
					if (list[0].ToLower() == "true")
					{
						InputDataList.Insert(Int16.Parse(RondeEdit) - 1, new InputDataList() { Ronde = RondeEdit, PuntenWij = list[1], PuntenZij = list[3], RoemWij = Int16.Parse(list[2]), RoemZij = Int16.Parse(list[4]), RoemWijSave = Int16.Parse(list[5]), RoemZijSave = Int16.Parse(list[6]) });
					}
			}

			PuntenWijEdit = null;
			PuntenZijEdit = null;
			RoemWijEdit = null;
			RoemZijEdit = null;


			EditWindowVisible = "false";
		}

		public Command CloseEditCommand { get; set; }

		public void CloseEditButton()
		{
			EditWindowVisible = "false";
		}


		public Command ResetCommand { get; set; }

		public void ResetButton()
		{
			InputDataList.Clear();
			RondeBind = "1";
			InputText = "Invoeren";
			PuntenWijBind = null;
			PuntenZijBind = null;
			RoemWijBind = "0";
			RoemZijBind = "0";
			OptionsVisibilty = false;
			RoundSelectVisible = "false";
			EditWindowVisible = "false";
			RoemWindowVisiblity = "false";
			InputWindowVisible = "false";
			ResultWindowVisible = "false";
			Totaal();
			SaveState.RemoveXml();

		}

		public Command CloseCommand { get; set; }

		public void CloseButton()
		{
			OptionsVisibilty = false;

		}

		public Command RoemWijCommand { get; set; }

		public void RoemWijButton()
		{
			Roemchoice = "wij";
			RoemWindowVisiblity = "true";
		}

		public Command RoemZijCommand { get; set; }

		public void RoemZijButton()
		{
			Roemchoice = "zij";
			RoemWindowVisiblity = "true";
		}

		public Command OneRoemCommand { get; set; }

		public void OneRoemButton()
		{

			RoemCalc(Roemchoice, 20);
			RoemWindowVisiblity = "false";
		}

		public Command TwoRoemCommand { get; set; }

		public void TwoRoemButton()
		{
			RoemCalc(Roemchoice, 40);
			RoemWindowVisiblity = "false";

		}

		public Command ThreeRoemCommand { get; set; }

		public void ThreeRoemButton()
		{
			RoemCalc(Roemchoice, 50);
			RoemWindowVisiblity = "false";

		}

		public Command FourRoemCommand { get; set; }

		public void FourRoemButton()
		{
			RoemCalc(Roemchoice, 70);
			RoemWindowVisiblity = "false";

		}

		public Command RoemCloseCommand { get; set; }

		public void RoemCloseButton()
		{
			RoemWindowVisiblity = "false";
			Roemchoice.DefaultIfEmpty();
		}

		public Command InputWindowCommand { get; set; }

		public void InputWindowButton()
		{
			RondeInput = RondeBind;
			PuntenWijInput = PuntenWijBind;
			PuntenZijInput = PuntenZijBind;
			RoemWijInput = RoemWijBind;
			RoemZijInput = RoemZijBind;
			InputWindowVisible = "true";
		}

		public Command CloseInputCommand { get; set; }

		public void CloseInputButton()
		{
			InputWindowVisible = "false";
		}

		public Command ResetInputCommand { get; set; }

		public void ResetInputButton()
		{
			PuntenWijBind = null;
			PuntenZijBind = null;
			RoemWijBind = "0";
			RoemZijBind = "0";
			InputWindowVisible = "false";
			RondeBind = (InputDataList.Count + 1).ToString();
			//some some field reset shenanigans
		}

		public Command OptionsCommand { get; set; }

		public void OptionsButton()
		{
			OptionsVisibilty = true;

		}


		public Command CloseResultCommand { get; set; }

		public void CloseResultButton()
        {
			ResultWindowVisible = "false";

		}
		#endregion

		//bindings
		#region Bindings
		private string _InputText = "Invoeren";

		public string InputText
		{
			get { return _InputText; }
			set { _InputText = value; OnPropertyChanged(nameof(InputText)); }

		}

		private string _PuntenWijBind;

		public string PuntenWijBind
		{
			get { return _PuntenWijBind; }
			set { _PuntenWijBind = value; OnPropertyChanged(nameof(PuntenWijBind)); }
		}

		private string _PuntenZijBind;

		public string PuntenZijBind
		{
			get { return _PuntenZijBind; }
			set { _PuntenZijBind = value; OnPropertyChanged(nameof(PuntenZijBind)); }
		}

		private string _RoemWijBind = "0";

		public string RoemWijBind
		{
			get { return _RoemWijBind; }
			set { _RoemWijBind = value; OnPropertyChanged(nameof(RoemWijBind)); }
		}

		private string _RoemZijBind = "0";

		public string RoemZijBind
		{
			get { return _RoemZijBind; }
			set { _RoemZijBind = value; OnPropertyChanged(nameof(RoemZijBind)); }
		}

		private string _RondeBind = "1";

		public string RondeBind
		{
			get { return _RondeBind; }
			set { _RondeBind = value; OnPropertyChanged(nameof(RondeBind)); }
		}

		private int _SubTotaalWijP;

		public int SubTotaalWijP
		{
			get { return _SubTotaalWijP; }
			set { _SubTotaalWijP = value; OnPropertyChanged(nameof(SubTotaalWijP)); }
		}

		private int _SubTotaalWijR;

		public int SubTotaalWijR
		{
			get { return _SubTotaalWijR; }
			set { _SubTotaalWijR = value; OnPropertyChanged(nameof(SubTotaalWijR)); }
		}

		private int _SubTotaalZijP;

		public int SubTotaalZijP
		{
			get { return _SubTotaalZijP; }
			set { _SubTotaalZijP = value; OnPropertyChanged(nameof(SubTotaalZijP)); }
		}

		private int _SubTotaalZijR;

		public int SubTotaalZijR
		{
			get { return _SubTotaalZijR; }
			set { _SubTotaalZijR = value; OnPropertyChanged(nameof(SubTotaalZijR)); }
		}

		private int _TotaalWij;

		public int TotaalWij
		{
			get { return _TotaalWij; }
			set { _TotaalWij = value; OnPropertyChanged(nameof(TotaalWij)); }
		}

		private int _TotaalZij;

		public int TotaalZij
		{
			get { return _TotaalZij; }
			set { _TotaalZij = value; OnPropertyChanged(nameof(TotaalZij)); }
		}

		private bool _OptionsVisibilty = false;

		public bool OptionsVisibilty
		{
			get { return _OptionsVisibilty; }
			set { _OptionsVisibilty = value; OnPropertyChanged(nameof(OptionsVisibilty)); }
		}

		private string _RoundSelectVisible = "false";

		public string RoundSelectVisible
		{
			get { return _RoundSelectVisible; }
			set { _RoundSelectVisible = value; OnPropertyChanged(nameof(RoundSelectVisible)); }
		}

		private string _EditWindowVisible = "false";

		public string EditWindowVisible
		{
			get { return _EditWindowVisible; }
			set { _EditWindowVisible = value; OnPropertyChanged(nameof(EditWindowVisible)); }

		}

		private string _RoemWindowVisiblity = "false";

		public string RoemWindowVisiblity
		{
			get { return _RoemWindowVisiblity; }
			set { _RoemWindowVisiblity = value; OnPropertyChanged(nameof(RoemWindowVisiblity)); }
		}

		private string _InputWindowVisible = "false";

		public string InputWindowVisible
		{
			get { return _InputWindowVisible; }
			set { _InputWindowVisible = value; OnPropertyChanged(nameof(InputWindowVisible)); }
		}

		private string _RondeInput;

		public string RondeInput
		{
			get { return _RondeInput; }
			set { _RondeInput = value; OnPropertyChanged(nameof(RondeInput)); }
		}

		private string _PuntenWijInput;

		public string PuntenWijInput
		{
			get { return _PuntenWijInput; }
			set { _PuntenWijInput = value; OnPropertyChanged(nameof(PuntenWijInput)); }
		}

		private string _PuntenZijInput;

		public string PuntenZijInput
		{
			get { return _PuntenZijInput; }
			set { _PuntenZijInput = value; OnPropertyChanged(nameof(PuntenZijInput)); }
		}

		private string _RoemWijInput;

		public string RoemWijInput
		{
			get { return _RoemWijInput; }
			set { _RoemWijInput = value; OnPropertyChanged(nameof(RoemWijInput)); }
		}

		private string _RoemZijInput;

		public string RoemZijInput
		{
			get { return _RoemZijInput; }
			set { _RoemZijInput = value; OnPropertyChanged(nameof(RoemZijInput)); }
		}

        private string _RondeInputEdit;

        public string RondeInputEdit
		{
            get { return _RondeInputEdit; }
            set { _RondeInputEdit = value; OnPropertyChanged(nameof(RondeInputEdit)); }
        }

        private string _RondeEdit;

        public string RondeEdit
		{
            get { return _RondeEdit; }
            set { _RondeEdit = value; OnPropertyChanged(nameof(RondeEdit)); }
        }

        private string _PuntenWijEdit;

        public string PuntenWijEdit
		{
            get { return _PuntenWijEdit; }
            set { _PuntenWijEdit = value; OnPropertyChanged(nameof(PuntenWijEdit)); }
        }

        private string _PuntenZijEdit;

        public string PuntenZijEdit
		{
            get { return _PuntenZijEdit; }
            set { _PuntenZijEdit = value; OnPropertyChanged(nameof(PuntenZijEdit)); }
        }

        private string _RoemWijEdit;

        public string RoemWijEdit
		{
            get { return _RoemWijEdit; }
            set { _RoemWijEdit = value; OnPropertyChanged(nameof(RoemWijEdit)); }
        }

        private string _RoemZijEdit;

        public string RoemZijEdit
		{
            get { return _RoemZijEdit; }
            set { _RoemZijEdit = value; OnPropertyChanged(nameof(RoemZijEdit)); }
        }

        private string _PuntenWijPlaceHolderEdit;

        public string PuntenWijPlaceHolderEdit
		{
            get { return _PuntenWijPlaceHolderEdit; }
            set { _PuntenWijPlaceHolderEdit = value; OnPropertyChanged(nameof(PuntenWijPlaceHolderEdit)); }
        }

        private string _PuntenZijPlaceHolderEdit;

        public string PuntenZijPlaceHolderEdit
		{
            get { return _PuntenZijPlaceHolderEdit; }
            set { _PuntenZijPlaceHolderEdit = value; OnPropertyChanged(nameof(PuntenZijPlaceHolderEdit)); }
        }

        private string _RoemWijPlaceHolderEdit;

        public string RoemWijPlaceHolderEdit
		{
            get { return _RoemWijPlaceHolderEdit; }
            set { _RoemWijPlaceHolderEdit = value; OnPropertyChanged(nameof(RoemWijPlaceHolderEdit)); }
        }

        private string _RoemZijPlaceHolderEdit;

        public string RoemZijPlaceHolderEdit
		{			
            get { return _RoemZijPlaceHolderEdit; }
            set { _RoemZijPlaceHolderEdit = value; OnPropertyChanged(nameof(RoemZijPlaceHolderEdit)); }
        }

		private String _ResultWindowVisible = "false";

        public String ResultWindowVisible
		{
            get { return _ResultWindowVisible; }
            set { _ResultWindowVisible = value; OnPropertyChanged(nameof(ResultWindowVisible)); }
        }

        #endregion


        // Logic
        #region Logic

        private void RoemCalc(string who, int roem)
        {
            switch (who)
            {
				case "wij":
					RoemWijBind = (Int16.Parse(RoemWijBind) + roem).ToString();
					break;
				case "zij":
					RoemZijBind = (Int16.Parse(RoemZijBind) + roem).ToString();
					break;
			}

			Roemchoice.DefaultIfEmpty();

		}

		private void Input(string r, string pw, int rw, string pz, int rz, int rws, int rzs)
		{
			var TempList = new InputDataList() { Ronde = r, PuntenWij = pw, RoemWij = rw, PuntenZij = pz, RoemZij = rz, RoemWijSave = rws, RoemZijSave = rzs};
			InputDataList.Add(TempList);
			RondeBind = (InputDataList.Count + 1).ToString();
			PuntenWijBind = null;
			PuntenZijBind = null;
			RoemWijBind = "0";
			RoemZijBind = "0";
			Totaal();

			SaveState.XmlSave(InputDataList);

		}


		private List<string> Calc(string pw, string pz, int rw, int rz)
        {

			string puntenwij = "0";
			string puntenzij = "0";
			int roemwij = 0;
			int roemzij = 0;
			bool changed = false;

			if (string.IsNullOrWhiteSpace(pw))
				pw = "null";

			if (string.IsNullOrWhiteSpace(pz))
				pz = "null";
			
			if (!pw.All(char.IsDigit) && !pz.All(char.IsDigit))
			{
				switch (pw.ToLower())
				{
					case "n":
						changed = true;
						puntenwij = "NAT";
						puntenzij = "162";
						roemwij = 0;
						roemzij = rw + rz;
						break;
					case "p":
						changed = true;
						puntenwij = "162";
						puntenzij = "PIT";
						roemwij = rw + rz + 100;
						roemzij = 0;
						break;
				}

				switch (pz.ToLower())
				{
					case "n":
						changed = true;
						puntenwij = "162";
						puntenzij = "NAT";
						roemwij = rw + rz;
						roemzij = 0;
						break;
					case "p":
						changed = true;
						puntenwij = "PIT";
						puntenzij = "162";
						roemwij = 0;
						roemzij = rw + rz + 100;
						break;
				}
			}
			else
			{
				changed = true;
				if (pw == "null")
				{
					puntenwij = (162 - Int16.Parse(pz)).ToString();
					puntenzij = pz;
				}
				else
				{
					puntenwij = pw;
					puntenzij = (162 - Int16.Parse(pw)).ToString();
				}

				roemwij = rw;
				roemzij = rz;
			}

            List<string> list = new List<string>
            {
                changed.ToString(),
                puntenwij,
                roemwij.ToString(),
                puntenzij,
                roemzij.ToString(),
                rw.ToString(),
                rz.ToString()
            };
            return list;

		}




		private void Totaal()
		{
			int pwij = 0;
			int rwij = 0;
			int pzij = 0;
			int rzij = 0;

			foreach (var i in InputDataList)
			{
				if (i.PuntenWij.All(char.IsDigit))
				{
					pwij = pwij + Int16.Parse(i.PuntenWij);
					rwij = rwij + i.RoemWij;
				}

				if (i.PuntenZij.All(char.IsDigit))
				{
					pzij = pzij + Int16.Parse(i.PuntenZij);
					rzij = rzij + i.RoemZij;
				}
			}

			SubTotaalWijP = pwij;
			SubTotaalWijR = rwij;
			SubTotaalZijP = pzij;
			SubTotaalZijR = rzij;
			TotaalWij = pwij + rwij;
			TotaalZij = pzij + rzij;

		}


		private List<string> Fix(int row, string pw, string pz, int rw, int rz)
		{

			int RoemWijUse = rw;
			int RoemZijUse = rz;

			if (PuntenWijPlaceHolderEdit == "PIT" || PuntenWijPlaceHolderEdit == "NAT" || PuntenZijPlaceHolderEdit == "PIT" || PuntenZijPlaceHolderEdit == "NAT")
			{ 
				RoemWijUse = InputDataList[row - 1].RoemWijSave;
				RoemZijUse = InputDataList[row - 1].RoemZijSave;
            }

			InputDataList.RemoveAt(row - 1);

			return Calc(pw, pz, RoemWijUse, RoemZijUse);

		}

		private void LoadXml()
		{ 
			ObservableCollection<InputDataList>	xmlLoaded = SaveState.XmlLoad();
			InputDataList.Clear(); 
			foreach (var i in xmlLoaded)
            {
				InputDataList.Add(i);
            }

			Totaal();
			RondeBind = (InputDataList.Count() + 1).ToString();

		}

		#endregion
	}
}



