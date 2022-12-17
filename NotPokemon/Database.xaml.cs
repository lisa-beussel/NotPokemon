using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

namespace NotPokemon
{
    /// <summary>
    /// Interaction logic for Database.xaml
    /// </summary>
    public partial class Database : Page
    {
        public Database()
        {
            InitializeComponent();
            //shows data
            Read();
        }

        //a list for all the monsters
        public List<Monster> MonsterList { get; private set; }

        public void Create()
        {
            using (DataContext context = new DataContext())
            {
                //takes input from the page
                var newName = nameinput.Text;
                var newType = typeinput.Text;
                var newHP = hpinput.Text;
                var newMP = mpinput.Text;
                var newAttack = attackinput.Text;
                bool newCBP = playerinput.IsChecked.Value;
                bool newCBE = enemyinput.IsChecked.Value;

                //making sure the values aren't empty
                if (newName != "" && newType != "" && newHP != "" && newMP != "" && newAttack != "")
                {
                    //creates a new monster and converts the values if needed
                    context.Monsters.Add(new Monster
                    {
                        Name = newName,
                        Type = char.Parse(newType),
                        HP = Int16.Parse(newHP),
                        MP = Int16.Parse(newMP),
                        Attack = Int16.Parse(newAttack),
                        CanBePlayer = newCBP,
                        CanBeEnemy = newCBE
                    });

                    //saves changes made to the database
                    context.SaveChanges();

                }
                else
                {
                    MessageBox.Show("It failed. At least one value is missing.");
                }
            }

            //changes are displayed right away
            Read();
        }

        //this method fetches the data from the database and displays it
        public void Read()
        {
            using (DataContext context = new DataContext())
            {
                MonsterList = context.Monsters.ToList();
                ListOfMonsters.ItemsSource = MonsterList;
            }
        }

        public void Update()
        {
            using (DataContext context = new DataContext())
            {
                //saves the monster that was selected on the list in a variable
                Monster selectedMonster = ListOfMonsters.SelectedItem as Monster;

                //saves the input in variables
                var newName = nameinput.Text;
                var newType = typeinput.Text;
                var newHP = hpinput.Text;
                var newMP = mpinput.Text;
                var newAttack = attackinput.Text;
                bool newCBP = playerinput.IsChecked.Value;
                bool newCBE = enemyinput.IsChecked.Value;

                //makes sure the variables aren't empty
                if (selectedMonster != null && newName != "" && newType != "" && newHP != "" && newMP != "" && newAttack != "")
                {
                    //selects the monster from the database that has the same id as the
                    //monster selected in the list
                    Monster editMonster = context.Monsters.Find(selectedMonster.Id);

                    //changes data
                    editMonster.Name = newName;
                    editMonster.Type = char.Parse(newType);
                    editMonster.HP = Int16.Parse(newHP);
                    editMonster.MP = Int16.Parse(newMP);
                    editMonster.Attack = Int16.Parse(newAttack);
                    editMonster.CanBePlayer = newCBP;
                    editMonster.CanBeEnemy = newCBE;

                    //saves changes
                    context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Something went wrong. Please make sure " +
                        "you selected a monster and filled in all the boxes.");
                }
            }

            Read();
        }

        //this method deletes a single monster from the database
        public void Delete()
        {
            using (DataContext context = new DataContext())
            {
                Monster selectedMonster = ListOfMonsters.SelectedItem as Monster;

                if (selectedMonster != null)
                {
                    Monster deleteMonster = context.Monsters.Find(selectedMonster.Id);

                    context.Remove(deleteMonster);
                    context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("No monster selected.");
                }
            }

            //changes are displayed right away
            Read();
        }

        //button for create/add
        private void add_Click(object sender, RoutedEventArgs e)
        {
            Create();
        }

        //button to go back to the main menu
        private void gotomm_Click(object sender, RoutedEventArgs e)
        {
            MainMenu mm = new MainMenu();
            this.NavigationService.Navigate(mm);
        }

        //button to update
        private void update_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        //button to delete
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }
    }
}
