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

            SetValues();
        }

        private string pName;
        private int pHPMax;
        private int pHPcurrent;
        private int pMPMax;
        private int pMPcurrent;
        private int pAttack;
        private char pType;

        private string eName;
        private int eHPMax;
        private int eHPcurrent;
        private int eMPMax;
        private int eMPcurrent;
        private int eAttack;
        private char eType;

        //how much HP does heal heal?
        private int healingpower = 40;

        //how many MP are regenerated each round?
        private int moreMP = 10;

        //how many MP are needed to heal?
        private int healMP = 15;

        //how many MP are needed to use magic?
        private int magicMP = 30;

        enum BattleState { pTurn, eTurn, Won, Lost }
        BattleState bs;

        public List<Monster> MonsterList { get; private set; }

        private void SetValues()
        {
            using (DataContext context = new DataContext())
            {
                MonsterList = context.Monsters.ToList();

                Random r = new Random();

                Monster playerMonster = MonsterList[r.Next(0, MonsterList.Count)];
                while(!playerMonster.CanBePlayer)
                {
                    playerMonster = MonsterList[r.Next(0, MonsterList.Count)];
                }

                Monster enemyMonster = MonsterList[r.Next(0, MonsterList.Count)];
                while (!enemyMonster.CanBeEnemy)
                {
                    enemyMonster = MonsterList[r.Next(0, MonsterList.Count)];
                }


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

        private void actionA_Click(object sender, RoutedEventArgs e)
        {
            if (bs == BattleState.pTurn)
            {
                HideButtons(true);
                eHPcurrent -= pAttack;
                //if enemy is dead
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

        private void actionM_Click(object sender, RoutedEventArgs e)
        {
            if (bs == BattleState.pTurn)
            {
                HideButtons(true);
                if (pMPcurrent >= magicMP)
                {
                    pMPcurrent -= magicMP;
                    pmp.Content = pMPcurrent + "/" + pMPMax;
                    eHPcurrent -= CalculateDamage(pType, eType);
                    //if enemy is dead
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

        private int CalculateDamage(char attacker, char victim)
        {
            // low damage, normal damage, high damage
            int ld = 10;
            int nd = 30;
            int hd = 50;

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

        private void actionH_Click(object sender, RoutedEventArgs e)
        {
            if (bs == BattleState.pTurn)
            {
                HideButtons(true);

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

        private void EnemyAttack()
        {
            pHPcurrent -= eAttack;
            //if player is dead
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

        private void EnemyMagic()
        {
            if (eMPcurrent >= magicMP)
            {
                eMPcurrent -= magicMP;
                emp.Content = eMPcurrent + "/" + eMPMax;
                pHPcurrent -= CalculateDamage(eType, pType);
                //if player is dead
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

        private void EnemyHeal()
        {
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

        private void NextStep()
        {
            switch (bs)
            {
                case BattleState.eTurn:
                    eMPcurrent += moreMP;
                    if (eMPcurrent > eMPMax)
                    {
                        eMPcurrent = eMPMax;
                    }
                    emp.Content = eMPcurrent + "/" + eMPMax;
                    EnemysTurn();
                    break;

                case BattleState.pTurn:
                    pMPcurrent += moreMP;
                    if (pMPcurrent > pMPMax)
                    {
                        pMPcurrent = pMPMax;
                    }
                    pmp.Content = pMPcurrent + "/" + pMPMax;
                    HideButtons(false);
                    break;

                case BattleState.Won:
                    Win win = new Win();
                    this.NavigationService.Navigate(win);
                    break;

                case BattleState.Lost:
                    Lost lost = new Lost();
                    this.NavigationService.Navigate(lost);
                    break;
            }
        }

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            MainMenu mm = new MainMenu();
            this.NavigationService.Navigate(mm);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            NextStep();
        }
    }
}
