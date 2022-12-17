using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        public Game()
        {
            InitializeComponent();

            //setting everything up
            SetValues();
        }

        //variables for the player
        private string pName;
        private int pHPMax;
        private int pHPcurrent;
        private int pMPMax;
        private int pMPcurrent;
        private int pAttack;
        private char pType;

        //variables for the enemy
        private string eName;
        private int eHPMax;
        private int eHPcurrent;
        private int eMPMax;
        private int eMPcurrent;
        private int eAttack;
        private char eType;

        //how much HP does 'heal' heal?
        private int healingpower = 40;

        //how many MP are regenerated each round?
        private int moreMP = 10;

        //how many MP are needed to heal?
        private int healMP = 15;

        //how many MP are needed to use magic?
        private int magicMP = 30;

        //keeping track of the state of the fight
        enum BattleState { pTurn, eTurn, Won, Lost }
        BattleState bs;

        //list of all monsters, used to pick a monster for the player and the enemy
        public List<Monster> MonsterList { get; private set; }

        //this method fetches the monsters from the database and picks one
        //for the player and one for the enemy. It then sets everything
        //(name, hp, mp) up and gets the fight ready.
        private void SetValues()
        {
            using (DataContext context = new DataContext())
            {
                MonsterList = context.Monsters.ToList();

                //the monsters for player and enemy are picked randomly.
                Random r = new Random();

                //making sure it picks a monster that can be the player's monster
                //see canbeplayer in database
                Monster playerMonster = MonsterList[r.Next(0, MonsterList.Count)];
                while(!playerMonster.CanBePlayer)
                {
                    playerMonster = MonsterList[r.Next(0, MonsterList.Count)];
                }

                //same goes for the enemy
                Monster enemyMonster = MonsterList[r.Next(0, MonsterList.Count)];
                while (!enemyMonster.CanBeEnemy)
                {
                    enemyMonster = MonsterList[r.Next(0, MonsterList.Count)];
                }

                //getting all the values from the database and saving them in variables
                pName = playerMonster.Name;
                pHPMax = playerMonster.HP;
                pHPcurrent = pHPMax;
                pMPMax = playerMonster.MP;
                pMPcurrent = pMPMax;
                pAttack = playerMonster.Attack;
                pType = playerMonster.Type;

                eName = enemyMonster.Name;
                eHPMax = enemyMonster.HP;
                eHPcurrent = eHPMax;
                eMPMax = enemyMonster.MP;
                eMPcurrent = eMPMax;
                eAttack = enemyMonster.Attack;
                eType = enemyMonster.Type;

            }

            pname.Content = pName;
            php.Content = pHPcurrent + "/" + pHPMax;
            pmp.Content = pMPcurrent + "/" + pMPMax;

            ename.Content = eName;
            ehp.Content = eHPcurrent + "/" + eHPMax;
            emp.Content = eMPcurrent + "/" + eMPMax;

            eventtext.Content = "Oh no! You encountered " + eName + "!";
            bs = BattleState.pTurn;

            HideButtons(false);
        }

        //player chooses 'attack'
        private void actionA_Click(object sender, RoutedEventArgs e)
        {
            if (bs == BattleState.pTurn)
            {
                HideButtons(true);
                eHPcurrent -= pAttack;
                //if enemy is killed
                if (eHPcurrent <= 0)
                {
                    eHPcurrent = 0;
                    ehp.Content = eHPcurrent + "/" + eHPMax;
                    bs = BattleState.Won;
                    eventtext.Content = pName + " attacked! It defeated " + eName + "! You have won!";
                }
                else
                {
                    ehp.Content = eHPcurrent + "/" + eHPMax;
                    eventtext.Content = pName + " attacked! What will " + eName + " do?";
                    bs = BattleState.eTurn;
                }
            }
        }

        //player chooses 'magic'
        private void actionM_Click(object sender, RoutedEventArgs e)
        {
            if (bs == BattleState.pTurn)
            {
                HideButtons(true);

                //only works if player has enough MP
                if (pMPcurrent >= magicMP)
                {
                    pMPcurrent -= magicMP;
                    pmp.Content = pMPcurrent + "/" + pMPMax;
                    eHPcurrent -= CalculateDamage(pType, eType);
                    //if enemy is killed
                    if (eHPcurrent <= 0)
                    {
                        eHPcurrent = 0;
                        ehp.Content = eHPcurrent + "/" + eHPMax;
                        bs = BattleState.Won;
                        eventtext.Content = pName + " used its magic against " +
                            eName + " and killed it! You have won!";
                    }
                    else
                    {
                        ehp.Content = eHPcurrent + "/" + eHPMax;
                        bs = BattleState.eTurn;
                        eventtext.Content = pName + " used its magic against " + eName + "!";
                    }
                }
                else
                {
                    bs = BattleState.eTurn;
                    eventtext.Content = pName + " doesn't have enough MP to do that.";
                }
            }
        }

        //calculating the amount of damage a magic attack does
        //3 types: fire, water, plant
        //works like rock, paper, scissor
        //fire beats plant, plant beats water, water beats fire
        private int CalculateDamage(char attacker, char victim)
        {
            // low damage, normal damage, high damage
            int ld = 10;
            int nd = 30;
            int hd = 50;

            //I used switches so it's easier to add more types in the future
            switch (attacker)
            {
                case 'f':
                    switch (victim)
                    {
                        case 'f':
                            return nd;

                        case 'w':
                            return ld;

                        case 'p':
                            return hd;

                        default:
                            MessageBox.Show("Error. victim type not found. It's " + victim);
                            return 0;
                    }

                case 'w':
                    switch (victim)
                    {
                        case 'f':
                            return hd;

                        case 'w':
                            return nd;

                        case 'p':
                            return ld;

                        default:
                            MessageBox.Show("Error. victim type not found. It's " + victim);
                            return 0;
                    }

                case 'p':
                    switch (victim)
                    {
                        case 'f':
                            return ld;

                        case 'w':
                            return hd;

                        case 'p':
                            return nd;

                        default:
                            MessageBox.Show("Error. victim type not found. It's " + victim);
                            return 0;
                    }

                default:
                    MessageBox.Show("Error. attacker type not found. It's " + attacker);
                    return 0;
            }
        }

        //if player chooses 'heal'
        private void actionH_Click(object sender, RoutedEventArgs e)
        {
            if (bs == BattleState.pTurn)
            {
                HideButtons(true);

                //only works if player has enough MP
                if (pMPcurrent >= healMP)
                {
                    pMPcurrent -= healMP;
                    pmp.Content = pMPcurrent + "/" + pMPMax;
                    pHPcurrent += healingpower;
                    if (pHPcurrent > pHPMax)
                    {
                        pHPcurrent = pHPMax;
                    }
                    php.Content = pHPcurrent + "/" + pHPMax;
                    bs = BattleState.eTurn;
                    eventtext.Content = pName + " healed itself. Now, it's " + eName + "'s turn.";
                }
                else
                {
                    bs = BattleState.eTurn;
                    eventtext.Content = pName + " can't heal itself due to too little MP.";
                }
            }
        }

        //enemy attacks
        private void EnemyAttack()
        {
            pHPcurrent -= eAttack;
            //if player is killed
            if (pHPcurrent <= 0)
            {
                pHPcurrent = 0;
                bs = BattleState.Lost;
                eventtext.Content = eName + " attacked! "
                    + pName + " is dead! You have lost!";
            }
            else
            {
                bs = BattleState.pTurn;
                NextStep();
                eventtext.Content = eName + " attacked! What will " + pName + " do?";
            }
            php.Content = pHPcurrent + "/" + pHPMax;
        }

        //enemy uses magic
        private void EnemyMagic()
        {
            //only works if enemy has enough MP
            if (eMPcurrent >= magicMP)
            {
                eMPcurrent -= magicMP;
                emp.Content = eMPcurrent + "/" + eMPMax;
                pHPcurrent -= CalculateDamage(eType, pType);
                //if player is killed
                if (pHPcurrent <= 0)
                {
                    pHPcurrent = 0;
                    bs = BattleState.Lost;
                    eventtext.Content = eName + " used its magic and killed "
                        + pName + "! You have lost!";
                }
                else
                {
                    bs = BattleState.pTurn;
                    NextStep();
                    eventtext.Content = eName +
                        " attacked with its magic! What will " + pName + " do?";
                }
                php.Content = pHPcurrent + "/" + pHPMax;
            }
            else
            {
                eventtext.Content = eName + " failed to attack with magic! What will " + pName + " do?";
                bs = BattleState.pTurn;
                NextStep();
            }
        }

        //enemy heals
        private void EnemyHeal()
        {
            //only works with enough MP
            //as of now, this will always be true as the enemy checks its MP before
            //deciding to heal or not. However, if something goes wrong
            //or changes are made to the game in the future, it is possible for the
            //enemy to fail at healing.
            if (eMPcurrent >= healMP)
            {
                eMPcurrent -= healMP;
                eHPcurrent += healingpower;
                emp.Content = eMPcurrent + "/" + eMPMax;
                if (eHPcurrent > eHPMax)
                {
                    eHPcurrent = eHPMax;
                }
                ehp.Content = eHPcurrent + "/" + eHPMax;
                bs = BattleState.pTurn;
                NextStep();
                eventtext.Content = eName + " healed itself. Now, it's " + pName + "'s turn.";
            }
            else
            {
                eventtext.Content = eName + " failed to heal itself. It's " + pName + "'s turn.";
                bs = BattleState.pTurn;
                NextStep();
            }
        }

        //AI decides what to do
        //will heal at low HP and with enough MP
        //else, it either attacks normally or with magic, decided randomly
        private void EnemysTurn()
        {
            if (eHPcurrent < eHPMax/2 && eMPcurrent >= healMP)
            {
                EnemyHeal();
            }
            else
            {
                Random random = new Random();
                int x = random.Next(1, 9);
                if (x <= 4)
                {
                    EnemyAttack();
                }
                else
                {
                    EnemyMagic();
                }
            }
        }

        //It's called 'HideButtons' because it used to hide them, but
        //I changed it to disabling them instead because I think that looks nicer.
        //Will disable the buttons for the player and enable a 'continue' button
        private void HideButtons(bool hide)
        {
            if(hide)
            {
                actionA.IsEnabled = false;
                actionM.IsEnabled = false;
                actionH.IsEnabled = false;
                OKButton.IsEnabled = true;
            }
            else
            {
                actionA.IsEnabled = true;
                actionM.IsEnabled = true;
                actionH.IsEnabled = true;
                OKButton.IsEnabled = false;
            }
        }

        //chooses the next step, depending on the battle state
        private void NextStep()
        {
            switch (bs)
            {
                //enemy's turn
                case BattleState.eTurn:
                    eMPcurrent += moreMP;
                    if (eMPcurrent > eMPMax)
                    {
                        eMPcurrent = eMPMax;
                    }
                    emp.Content = eMPcurrent + "/" + eMPMax;
                    EnemysTurn();
                    break;

                //player's turn
                case BattleState.pTurn:
                    pMPcurrent += moreMP;
                    if (pMPcurrent > pMPMax)
                    {
                        pMPcurrent = pMPMax;
                    }
                    pmp.Content = pMPcurrent + "/" + pMPMax;
                    HideButtons(false);
                    break;

                //player has won; goes to winning screen
                case BattleState.Won:
                    Win win = new Win();
                    this.NavigationService.Navigate(win);
                    break;

                //player has lost; goes to losing screen
                case BattleState.Lost:
                    Lost lost = new Lost();
                    this.NavigationService.Navigate(lost);
                    break;
            }
        }

        //quits the game and returns to main menu
        private void quit_Click(object sender, RoutedEventArgs e)
        {
            MainMenu mm = new MainMenu();
            this.NavigationService.Navigate(mm);
        }

        //goes to the next step once player clicks the 'okay'/'continue' button
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            NextStep();
        }
    }
}
