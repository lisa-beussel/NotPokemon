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
            Read();
        }

        public List<Monster> MonsterList { get; private set; }

        public void Create()
        {
            using (DataContext context = new DataContext())
            {

                var newName = nameinput.Text;
                var newType = typeinput.Text;
                var newHP = hpinput.Text;
                var newMP = mpinput.Text;
                var newAttack = attackinput.Text;
                bool newCBP = playerinput.IsChecked.Value;
                bool newCBE = enemyinput.IsChecked.Value;

                if (newName != "" && newType != "" && newHP != "" && newMP != "" && newAttack != "")
                {
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

                    context.SaveChanges();

                }
                else
                {
                    MessageBox.Show("It failed. At least one value is missing.");
                }
            }

            Read();
        }

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
                Monster selectedMonster = ListOfMonsters.SelectedItem as Monster;

                var newName = nameinput.Text;
                var newType = typeinput.Text;
                var newHP = hpinput.Text;
                var newMP = mpinput.Text;
                var newAttack = attackinput.Text;
                bool newCBP = playerinput.IsChecked.Value;
                bool newCBE = enemyinput.IsChecked.Value;

                if (selectedMonster != null && newName != "" && newType != "" && newHP != "" && newMP != "" && newAttack != "")
                {
                    Monster editMonster = context.Monsters.Find(selectedMonster.Id);

                    editMonster.Name = newName;
                    editMonster.Type = char.Parse(newType);
                    editMonster.HP = Int16.Parse(newHP);
                    editMonster.MP = Int16.Parse(newMP);
                    editMonster.Attack = Int16.Parse(newAttack);
                    editMonster.CanBePlayer = newCBP;
                    editMonster.CanBeEnemy = newCBE;

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

            Read();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            Create();
        }

        private void gotomm_Click(object sender, RoutedEventArgs e)
        {
            MainMenu mm = new MainMenu();
            this.NavigationService.Navigate(mm);
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }
    }
}
