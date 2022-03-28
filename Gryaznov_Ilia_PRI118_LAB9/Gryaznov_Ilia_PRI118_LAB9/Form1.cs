using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using System.Threading;
using System.Media;
using System.Windows.Media;
using System.IO;
using Tao.DevIl;

namespace Gryaznov_Ilia_PRI118_LAB9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }
        private Explosion BOOOOM_1 = new Explosion(1, 10, 1, 300, 100);
        float global_time = 0;
        double angle=3, angleX=-96,angleY = 0, angleZ=-30;
        double sizeX=1, sizeY=1, sizeZ=1;
        double translateX=-9, translateY=30, translateZ=-10;
        double serpent = -63; double deltaSerpent = 0.15;
        bool serpentflag = false; bool hummerFlag = false; bool hummerFlag2 = false; bool lightsound = true; bool wind = false; bool driveHummer = false;
        double cameraSpeed;
        double testRot;
        point hummer, point;
        double YdeltaHummer = 0.377;
        double XdeltaHummer = 0.4;
        double ZdeltaHummer = 0.6;
        MediaPlayer background, light, thunder, roar, shtorm;
        double sailScale = 1;
        double Yship = 0.1;

        int imageId;
        uint mGlTextureObject;

        private void Form1_Load(object sender, EventArgs e)
        {

            background = new MediaPlayer(); 
            light = new MediaPlayer();
            thunder = new MediaPlayer();
            roar = new MediaPlayer();
            shtorm = new MediaPlayer();
            background.Open(new Uri(@"../../soudns\back.wav", System.UriKind.Relative));
            light.Open(new Uri(@"../../soudns\light.wav", System.UriKind.Relative));
            thunder.Open(new Uri(@"../../soudns\thunder.wav", System.UriKind.Relative));
            roar.Open(new Uri(@"../../soudns\roar.wav", System.UriKind.Relative));
            shtorm.Open(new Uri(@"../../soudns\shtorm.wav", System.UriKind.Relative));
            background.Play();

            button2.Enabled = false;
            hummer = new point(45, 140, 88);
            point = new point(35.7, 137, 87.8);
            trackBar1.Value = 10;
            // инициализация OpenGL, много раз комментированная ранее
            // инициализация бибилиотеки glut
            Glut.glutInit();
            // инициализация режима экрана
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);

            Il.ilInit();
            Il.ilEnable(Il.IL_ORIGIN_SET);

            // установка цвета очистки экрана (RGBA)
            Gl.glClearColor(255, 255, 255, 1);

            // установка порта вывода
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);

            // активация проекционной матрицы
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // очистка матрицы
            Gl.glLoadIdentity();

            Glu.gluPerspective(60, (float)AnT.Width / (float)AnT.Height, 0.1, 800);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Gl.glEnable(Gl.GL_DEPTH_TEST);
           // Gl.glEnable(Gl.GL_LIGHTING);
           //Gl.glEnable(Gl.GL_LIGHT0);

            // ЗАГУРУЗКА ИЗОБРАЖЕНИЯ ПО УМОЛЧАНИЮ
            Il.ilGenImages(1, out imageId);
            Il.ilBindImage(imageId);
            if (Il.ilLoadImage("../../texture/list.png"))
            {
                int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
                int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
                switch (bitspp)
                {
                    case 24:
                        mGlTextureObject = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
                        break;
                    case 32:
                        mGlTextureObject = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
                        break;
                }
            }
            Il.ilDeleteImages(1, ref imageId);


            comboBox1.SelectedIndex = 0;
            label14.Text = trackBar1.Value.ToString();

            // начало визуализации (активируем таймер)
            RenderTimer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serpentflag = true;
            button2.Enabled = true;
            AnT.Focus();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label14.Text = trackBar1.Value.ToString();
            AnT.Focus();
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            wind = checkBox1.Checked;
            AnT.Focus();
        }

        private void AnT_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.W)
            {
                translateY -= cameraSpeed;

            }
            if (e.KeyCode == Keys.S)
            {
                translateY += cameraSpeed;
            }
            if (e.KeyCode == Keys.A)
            {
                translateX += cameraSpeed;
            }
            if (e.KeyCode == Keys.D)
            {
                translateX -= cameraSpeed;
  
            }
            if (e.KeyCode == Keys.ControlKey)
            {
                translateZ += cameraSpeed;

            }
            if (e.KeyCode == Keys.Space)
            {
                translateZ -= cameraSpeed;
            }
        

            if (e.KeyCode == Keys.Q)
            {
                switch(comboBox1.SelectedIndex)
                {
                    case 0:
                        angleX +=angle;
                       
                        break;
                    case 1:
                        angleY += angle;
                       
                        break;
                    case 2:
                        angleZ += angle;
                       
                        break;
                    default:
                        break;
                }
            }
            if (e.KeyCode == Keys.E)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        angleX -= angle;
                        break;
                    case 1:
                        angleY -= angle;
                        break;
                    case 2:
                        angleZ -= angle;
                        break;
                    default:
                        break;
                }
            }
            if (e.KeyCode == Keys.Z)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        sizeX += 0.1;
                        break;
                    case 1:
                        sizeY += 0.1;
                        break;
                    case 2:
                        sizeZ += 0.1;
                        break;
                    default:
                        break;
                }
            }
            if (e.KeyCode == Keys.X)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        sizeX -= 0.1;
                        break;
                    case 1:
                        sizeY -= 0.1;
                        break;
                    case 2:
                        sizeZ -= 0.1;
                        break;
                    default:
                        break;
                }
            }
            if(e.KeyCode == Keys.I && sailScale>-1)
            {
                sailScale -= 0.1;
            }
            if (e.KeyCode == Keys.K && sailScale < 1)
            {
                sailScale += 0.1;
            }
            if (e.KeyCode == Keys.Y && driveHummer)
            {
                point.y += 1;
                hummer.y += 1;
            }
            if (e.KeyCode == Keys.H && driveHummer)
            {
                point.y -= 1;
                hummer.y -= 1;
            }
            if (e.KeyCode == Keys.J && driveHummer && point.x<-50)
            {
                point.x += 1;
                hummer.x += 1;
            }
            if (e.KeyCode == Keys.G && driveHummer)
            {
                point.x -= 1;
                hummer.x -= 1;
            }
            if (e.KeyCode == Keys.U && driveHummer)
            {
                point.z += 1;
                hummer.z += 1;
            }
            if (e.KeyCode == Keys.T && driveHummer && point.z>110)
            {
                point.z -= 1;
                hummer.z -= 1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            thunder.Play();
            hummerFlag = true;
            AnT.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            global_time += (float)RenderTimer.Interval / 1000;
            cameraSpeed = (double)trackBar1.Value/10;
            Draw();
        }
        // функция отрисовки
        private void Draw()
        {
            // очистка буфера цвета и буфера глубины
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glClearColor(255, 255, 255, 1);
            // очищение текущей матрицы
            Gl.glLoadIdentity();
            // помещаем состояние матрицы в стек матриц
           
            Gl.glPushMatrix();           
            Gl.glRotated(angleX, 1, 0, 0); Gl.glRotated(angleY, 0, 1, 0); Gl.glRotated(angleZ, 0, 0, 1);
            Gl.glTranslated(translateX, translateY, translateZ);
            Gl.glScaled(sizeX, sizeY, sizeZ);
            Gl.glColor3f(0.07f, 0.04f, 0.56f);
            BOOOOM_1.Calculate(global_time);
            //////////////////////////
            ///////////////////////////
            ////////ПРИСТАНЬ/////////
            ////////////////////////
            ////////////////////////
            Gl.glPushMatrix();
            for (int i = 0; i < 21; i++)
            {
                block(-1, -4, 0, 0.3f, 0.1f, 0, 5);
                if (i % 2 == 0)
                {
                    Gl.glPushMatrix();
                        Gl.glScaled(0.2, 0.5, 20);
                        block(-5, -10, -0.38, 0.3f, 0.1f, 0, 5);
                        block(19, -10, -0.38, 0.3f, 0.1f, 0, 5);
                    Gl.glPopMatrix();
                    if (i != 20)
                    {
                        Gl.glPushMatrix();
                        Gl.glLineWidth(8f);
                        Gl.glTranslated(0, 1, 7.6);
                        Gl.glRotated(10, 1, 0, 0);
                        circle(-0.4, -4, 5, 10, "x", Math.PI / 8, false, false,20,10);
                        circle(4.4, -4, 5, 10, "x", Math.PI / 8, false, false,20,10);
                        Gl.glPopMatrix();
                    }
                }
                Gl.glTranslated(0, 2, 0);
            }
            Gl.glPopMatrix();
            Gl.glPushMatrix();
            for (int i = 0; i < 21; i++)
            {
                block(-31, -4, 0, 0.3f, 0.1f, 0, 5);
                if (i % 2 == 0)
                {
                    Gl.glPushMatrix();
                        Gl.glScaled(0.2, 0.5, 20);
                        block(-155, -10, -0.38, 0.3f, 0.1f, 0, 5);
                        block(-131, -10, -0.38, 0.3f, 0.1f, 0, 5);
                    Gl.glPopMatrix();
                    if (i != 20)
                    {
                        Gl.glPushMatrix();
                        Gl.glLineWidth(8f);
                        Gl.glTranslated(0, 1, 7.6);
                        Gl.glRotated(10, 1, 0, 0);
                        circle(-30.4, -4, 5, 10, "x", Math.PI / 8, false, false,20,10);
                        circle(-25.6, -4, 5, 10, "x", Math.PI / 8, false, false,20,10);
                        Gl.glPopMatrix();
                    }
                }
                Gl.glTranslated(0, 2, 0);
            }
            Gl.glPopMatrix();
            //////////////////////////
            ///////////////////////////
            ////////МОРЕ МОРЕ/////////
            ////////////////////////
            ////////////////////////
            Gl.glColor3f(0.066f, 0.376f, 0.7f);
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glVertex3d(200,200,-5);
            Gl.glVertex3d(-200, 200, -5);
            Gl.glVertex3d(-200, -10, -5);
            Gl.glVertex3d(200, -10, -5);
            Gl.glEnd();

            //////////////////////////
            ///////////////////////////
            ////////МЬЕЛНИР/////////
            ////////////////////////
            ////////////////////////
            if (hummerFlag)
            {
                if (hummer.x < 60 && !hummerFlag2)
                {
                    //
                    hummer.x = hummer.x + XdeltaHummer;
                    point.x = point.x + XdeltaHummer;
                    hummer.y = hummer.y - YdeltaHummer;
                    point.y = point.y - YdeltaHummer;
                    hummer.z = hummer.z - ZdeltaHummer;
                    point.z = point.z - ZdeltaHummer;
                }
                if(hummer.x>60)
                {
                    hummerFlag2 = true;
                    ZdeltaHummer = 0.35;
                    YdeltaHummer = 0.45;
                    XdeltaHummer = 0.8;
                }
                if (hummerFlag2 && hummer.x>-15)
                {
                    testRot = testRot + 16;
                    hummer.x = hummer.x - XdeltaHummer;
                    point.x = point.x - XdeltaHummer;
                    hummer.y = hummer.y - YdeltaHummer;
                    point.y = point.y - YdeltaHummer;
                    hummer.z = hummer.z - ZdeltaHummer;
                    point.z = point.z - ZdeltaHummer;
                    ZdeltaHummer -= 0.003;
                }
                if(lightsound && hummer.x <= -15) 
                {
                    BOOOOM_1.SetNewPosition(-25, 78, 44);
                    BOOOOM_1.SetNewPower(60);
                    BOOOOM_1.Boooom(global_time);
                    lightsound = false;
                    light.Play();
                    deltaSerpent = -0.2;
                    serpent = -1;
                    roar.Play();
                }
                if (hummer.x <= -15 && hummer.z<=110) 
                {
                    ZdeltaHummer = 0.6;

                    hummer.y = hummer.y + YdeltaHummer;
                    point.y = point.y + YdeltaHummer;
                    hummer.z = hummer.z + ZdeltaHummer;
                    point.z = point.z + ZdeltaHummer;
                }
                if(hummer.z >= 110)
                {
                    driveHummer = true;
                }

            }
            testRot = testRot + 8;
            Gl.glPushMatrix();
            Gl.glRotated(testRot, point.x, point.y, point.z);
            Gl.glTranslated(hummer.x, hummer.y, hummer.z);
            Gl.glPushMatrix();
                Gl.glScaled(0.2, 0.3, 1);
                block(-50, -10, -0.38, 0, 0.22f, 0.45f, 2);
            Gl.glPopMatrix();
            Gl.glPushMatrix();
                Gl.glScaled(0.04, 0.1, 3);
                Gl.glTranslated(-188, -22, -0.18);
                block(-50, -10, -0.38, 0.48f, 0.28f, 0, 2);
            Gl.glPopMatrix();
            Gl.glPopMatrix();


            //////////////////////////
            ///////////////////////////
            ////////ЙОРМУНГАНД/////////
            //////////////////////// 
            ////////////////////////
            point[] krug = new point[40];
            double grad = Math.PI * 2 / 40;
            for (int i = 0; i < 40; i++)
            {
                krug[i] = new point();
                krug[i].x = Math.Cos(grad * i) * 15 -40; ;
                krug[i].y = Math.Sin(grad * i) * 15 + 100;
                krug[i].z = -10;
            }
            Gl.glPushMatrix();
            if (serpentflag && serpent < 0 && serpent>-64)
            {
                shtorm.Play();
                serpent += deltaSerpent;
            }
            Gl.glTranslated(0, 0, serpent); 
            krug =cilinder(krug, -40, 100, 0, 15,0,"z", false, 0.24f, 1f, 0.89f, 0, 0, 0);
            krug=cilinder(krug, -40, 100, 10, 15, 0, "z", false, 0.24f, 1f, 0.89f, 0, 0, 0);
            krug = cilinder(krug, -40, 100, 20, 15, 0, "z", false, 0.24f, 1f, 0.89f, 0, 0, 0);
            krug = cilinder(krug, -40, 100, 30, 15, 0, "z", false, 0.24f, 1f, 0.89f, 0, 0, 0);
            krug = cilinder(krug , -40, 95, 40, 15, 30, "z", false, 0.24f, 1f, 0.89f, 0, 0, 0);
            krug = cilinder(krug, -40, 80, 45, 15, -30, "y", false, 0.24f, 1f, 0.89f, 0, 0, 0);
            krug = cilinder(krug, -40, 65, 45, 10, 0, "y", false, 0.24f, 1f, 0.89f, 0, 0, 0);
            krug = cilinder(krug, -40, 50, 38, 5, 10, "y", true, 0.24f, 1f, 0.89f, 0, 0, 0);
            krug = cilinder(krug, -40, 47, 38, 2, 10, "y", true, 0.24f, 1f, 0.89f, 0, 0, 0);
            krug = cilinder(krug, -40, 46, 38, 0.2, 20, "y", true, 0.24f, 1f, 0.89f, 0, 0, 0);
            Gl.glPopMatrix();

            ///ГЛАЗА ЗУБЫ ПРОЧЕЕ///
            Gl.glPushMatrix();
            if (serpentflag && serpent < 0)
            {
                serpent += deltaSerpent;
            }
            Gl.glTranslated(0, 0, serpent);  
            Gl.glColor3f(0, 0, 0);
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);          
            Gl.glVertex3d(-31, 63, 48);
            Gl.glVertex3d(-32.5, 56, 42.5);
            Gl.glVertex3d(-30, 63, 41.5);         
            Gl.glEnd();
            Gl.glPushMatrix();
            Gl.glTranslated(-19, 0, 0);
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glVertex3d(-30, 63, 48);
            Gl.glVertex3d(-28.5, 56, 42.5);
            Gl.glVertex3d(-30.8, 63, 41.5);
            Gl.glEnd();
            Gl.glPopMatrix();
            Gl.glPopMatrix();

           
            Gl.glPushMatrix();
            if (serpentflag && serpent < 0)
            {
                serpent += deltaSerpent;
            }
            Gl.glTranslated(0, 0, serpent);
            Gl.glScaled(1, 1.7, 1);
            point[] teeth =circle(-40, 36, 40, 9, "z", Math.PI , false, false, 10, 5);
            for (int i = 0; i < teeth.Length - 1; i++)
            {
                Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                Gl.glVertex3d(teeth[i].x, teeth[i].y, teeth[i].z);
                Gl.glVertex3d(teeth[i + 1].x, teeth[i + 1].y, teeth[i + 1].z - 3);
                Gl.glVertex3d(teeth[i + 1].x, teeth[i + 1].y, teeth[i + 1].z);
                Gl.glEnd();
            }
            Gl.glPopMatrix();


            
            Gl.glPushMatrix();
            if (serpentflag && serpent < 0)
            {
                serpent += deltaSerpent;
            }
            Gl.glTranslated(0, 0, serpent);
            Gl.glRotated(30, 1, 0, 0);
            Gl.glScaled(1, 1.7, 1);           
            point[] teeth1 = circle(-39.5, 44, 3.5, 9, "z", Math.PI, false, false, 10, 5);
            for (int i = 9; i > 1; i--)
            {
                Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                Gl.glVertex3d(teeth1[i].x, teeth1[i].y, teeth1[i].z);
                Gl.glVertex3d(teeth1[i - 1].x, teeth1[i - 1].y, teeth1[i - 1].z +3);
                Gl.glVertex3d(teeth1[i - 1].x, teeth1[i - 1].y, teeth1[i - 1].z);
                Gl.glEnd();
            }
            
            Gl.glPopMatrix();
            //Gl.glPopMatrix(); //НА ВСЯКИЙ



            //////////////////////////
            ///////////////////////////
            ////////ДРАККАР/////////
            ////////////////////////
            ////////////////////////  
            Gl.glPushMatrix();
            if (wind)
            {
                Yship -= sailScale/2.5;
            }
            if (Yship > 0)
                Gl.glTranslated(0, Yship, 0);
            else
                Yship = 0;
            Gl.glPushMatrix();
            Gl.glScaled(1.4, 1, 1.3);
            Gl.glTranslated(4, 0, 1);
            point[] helpmeplease = new point[40];
            double rot = Math.PI / 40;
            for (int i = 0; i < 40; i++)
            {
                helpmeplease[i] = new point();
                helpmeplease[i].z=-Math.Cos(rot * i) * 6 -4.5;
                helpmeplease[i].y = -Math.Sin(rot * i) * 6;
                helpmeplease[i].x = -13;
            }
            Gl.glPushMatrix();          
            Gl.glRotated(90, 1, 0, 0);
            Gl.glScaled(1, 1, 3.25);
            helpmeplease=connect(helpmeplease, -11, 7.43, -4.5, 6, Math.PI/6, 0.58f, 0.48f, 0, false);
            helpmeplease=connect(helpmeplease, -9.18, 9.85, -4.5, 6, Math.PI /4.8, 0.58f, 0.48f, 0, false);
            connect(helpmeplease, -1.35, 16, -4.5, 6, Math.PI /3.5, 0.58f, 0.48f, 0, false);       
            Gl.glPopMatrix();

           

            for (int i = 0; i < 40; i++)
            {
                helpmeplease[i] = new point();
                helpmeplease[i].z = -Math.Cos(rot * i) * 6 - 4.5;
                helpmeplease[i].y = -Math.Sin(rot * i) * 6;
                helpmeplease[i].x = -13;
            }

            Gl.glPushMatrix();
            Gl.glRotated(90, 1, 0, 0);
            Gl.glScaled(1, 1, 3.25);
            Gl.glScaled(-1, 1, 1);
            Gl.glTranslated(26, 0, 0);
            helpmeplease = connect(helpmeplease, -11, 7.43, -4.5, 6, Math.PI / 6, 0.58f, 0.48f, 0, false);
            helpmeplease = connect(helpmeplease, -9.18, 9.85, -4.5, 6, Math.PI / 4.8, 0.58f, 0.48f, 0, false);
            connect(helpmeplease, -1.35, 16, -4.5, 6, Math.PI / 3.5, 0.58f, 0.48f, 0, false);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
            
            //ЩИТЫ
            Gl.glPushMatrix();
            fullcircle(-6.5, 12, 0.6, 1.5, 40, 0.355f,0,0);
            Gl.glRotated(10, 0, 0, 1);
            fullcircle(-3.1, 22, 0.6, 1.5, 40, 0.355f, 0, 0);
            Gl.glRotated(10, 0, 0, 1);
            fullcircle(1.7, 27, 0.6, 1.5, 40, 0.355f, 0, 0);
            Gl.glRotated(10, 0, 0, 1);
            fullcircle(7.3, 32, 0.6, 1.5, 40, 0.355f, 0, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(-1, 26, 0);
            Gl.glScaled(1, -1, 1);
            Gl.glRotated(10, 0, 0, 1);
            fullcircle(-2.56, 22, 0.6, 1.5, 40, 0.355f, 0, 0);
            Gl.glRotated(10, 0, 0, 1);
            fullcircle(1.7, 27, 0.6, 1.5, 40, 0.355f, 0, 0);
            Gl.glRotated(10, 0, 0, 1);
            Gl.glPopMatrix();


            Gl.glPushMatrix();
            Gl.glScaled(-1, 1, 1);
            Gl.glTranslated(25.2, 0, 0);
            Gl.glPushMatrix();
            fullcircle(-6.5, 12, 0.6, 1.5, 40, 0.355f, 0, 0);
            Gl.glRotated(10, 0, 0, 1);
            fullcircle(-3.1, 22, 0.6, 1.5, 40, 0.355f, 0, 0);
            Gl.glRotated(10, 0, 0, 1);
            fullcircle(1.7, 27, 0.6, 1.5, 40, 0.355f, 0, 0);
            Gl.glRotated(10, 0, 0, 1);
            fullcircle(7.3, 32, 0.6, 1.5, 40, 0.355f, 0, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(-1, 26, 0);
            Gl.glScaled(1, -1, 1);
            Gl.glRotated(10, 0, 0, 1);
            fullcircle(-2.56, 22, 0.6, 1.5, 40, 0.355f, 0, 0);
            Gl.glRotated(10, 0, 0, 1);
            fullcircle(1.7, 27, 0.6, 1.5, 40, 0.355f, 0, 0);
            Gl.glRotated(10, 0, 0, 1);
            Gl.glPopMatrix();

            
            //ДНО, бочки, ящики
            Gl.glPushMatrix();
            Gl.glTranslated(-32.6, 14.8, 0);
            Gl.glScaled(1, 3.25, 1);
            Gl.glRotated(90, 0, 1, 0);
            fullcircle(0, 0, 20, 5.75, 40, 0.58f, 0.48f, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glScaled(0.5, 1, 3);
            block(-25, 1, 0.03, 0.8f, 0.8f, 0, 5);
            Gl.glScaled(0.8, 0.8, 0.8);
            block(-30, 1, 0.673, 0.8f, 0.8f, 0, 5);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            for (int i = 0; i < 40; i++)
            {
                krug[i].x = Math.Cos(grad * i) * 1 ;
                krug[i].y = Math.Sin(grad * i) * 1;
                krug[i].z = 6;
            }
            Gl.glTranslated(-10, 5, -5.95);
            krug =cilinder(krug, 0, 0, 7, 1.3, 0, "z", false, 0.157f, 0.067f, 0, 0, 0, 0);
            krug = cilinder(krug, 0, 0, 8, 1, 0, "z", false, 0.157f, 0.067f, 0, 0, 0, 0);
            Gl.glRotated(90, 0, 1, 0);
            fullcircle(-6, 0, 0, 1, 40, 0.157f, 0.067f, 0);
            Gl.glRotated(-90, 0, 1, 0);
            Gl.glTranslated(0, 8, 0);
            for (int i = 0; i < 40; i++)
            {
                krug[i].x = Math.Cos(grad * i) * 1;
                krug[i].y = Math.Sin(grad * i) * 1;
                krug[i].z = 6;
            }
            krug = cilinder(krug, 0, 0, 7, 1.3, 0, "z", false, 0.157f, 0.067f, 0, 0, 0, 0);
            krug = cilinder(krug, 0, 0, 8, 1, 0, "z", false, 0.157f, 0.067f, 0, 0, 0, 0);
            Gl.glRotated(90, 0, 1, 0);
            fullcircle(-6, 0, 0, 1, 40, 0.157f, 0.067f, 0);
            Gl.glRotated(-90, 0, 1, 0);
            Gl.glTranslated(-7, 0, 0);
            for (int i = 0; i < 40; i++)
            {
                krug[i].x = Math.Cos(grad * i) * 1;
                krug[i].y = Math.Sin(grad * i) * 1;
                krug[i].z = 6;
            }
            krug = cilinder(krug, 0, 0, 7, 1.3, 0, "z", false, 0.157f, 0.067f, 0, 0, 0, 0);
            krug = cilinder(krug, 0, 0, 8, 1, 0, "z", false, 0.157f, 0.067f, 0, 0, 0, 0);
            Gl.glRotated(90, 0, 1, 0);
            fullcircle(-6, 0, 0, 1, 40, 0.157f, 0.067f, 0);
            Gl.glPopMatrix();
            
            //МАЧТА
            for (int i = 0; i < 40; i++)
            {
                krug[i] = new point();
                krug[i].x = Math.Cos(grad * i) * 0.5 -12.5;
                krug[i].y = Math.Sin(grad * i) * 0.5 +12;
                krug[i].z = 0.1;
            }
            Gl.glPushMatrix();
            cilinder(krug, -12.5, 12, 23, 0.5, 0, "z", false, 0.58f, 0.48f, 0, 0, 0, 0);
            Gl.glTranslated(-19.5, -0.1, 7.5);
            Gl.glRotated(90, 0, 1, 0);           
            cilinder(krug, -12.5, 12, 15, 0.5, 0, "z", false, 0.58f, 0.48f, 0, 0, 0, 0);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(-19.5, -0.1, -8);
            Gl.glRotated(90, 0, 1, 0);
            cilinder(krug, -12.5, 12, 15, 0.5, 0, "z", false, 0.58f, 0.48f, 0, 0, 0, 0);
            Gl.glPopMatrix();


            for (int i = 0; i < 40; i++)
            {
                helpmeplease[i] = new point();
                helpmeplease[i].z = -Math.Cos(rot * i) * 7.5 +20;
                helpmeplease[i].y = -Math.Sin(rot * i) * 7.5;
                helpmeplease[i].x = 0;
            }
            Gl.glPushMatrix();
            Gl.glTranslated(-17.5, 11.4, -8);
            Gl.glScaled(1, sailScale, 1);
            connect(helpmeplease, 10, 0, 20, 7.5, 0, 0.73f, 0.15f, 0, false);
            Gl.glPopMatrix();        
            Gl.glPopMatrix();
            Gl.glPopMatrix();



            //////////////////////////
            ///////////////////////////
            ////////ИГГДРАСИЛЬ/////////
            ////////////////////////
            ////////////////////////  

            for (int i = 0; i < 40; i++)
            {
                krug[i] = new point();
                krug[i].x = 170;
                krug[i].y = Math.Sin(grad * i) * 20 + 120;
                krug[i].z = Math.Cos(grad * i) * 20 + 80;
            }
            Gl.glPushMatrix();
            Gl.glTranslated(0, 70, 0);
            krug = cilinder(krug, 140, 110, 75, 20, 0, "x", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, 110, 100, 70, 18, 0, "x", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, 80, 90, 65, 15, 0, "x", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, 50, 80, 65, 17, 0, "x", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, 20, 70, 70, 24, 0, "x", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);/////////////придаточные отсюда
            krug = cilinder(krug, -10, 60, 70, 18, 0, "x", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -30, 50, 72, 16, 0, "x", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -60, 40, 72, 13, 0, "x", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -90, 30, 70, 8, 0, "x", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -120, 20, 65, 4, 0, "x", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -130, 20, 65, 3, 0, "x", true, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -140, 20, 65, 2, 0, "x", true, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            for (int i = 0; i < 40; i++)
            {
                krug[i].x = Math.Cos(grad * i) *13  + 30;
                krug[i].y = 70;
                krug[i].z = Math.Sin(grad * i) * 13 + 70;
            }
            krug = cilinder(krug, 10, 30, 65, 13, 0, "y", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -20, 0, 65, 11, 0, "y", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -45, -25, 60, 9, 0, "y", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -65, -45, 63, 5, 0, "y", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -80, -60, 62, 4, 0, "y", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -90, -70, 61, 3, 0, "y", true, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -100, -80, 60, 2, 0, "y", true, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            Gl.glTranslated(0, 140, 0);
            Gl.glScaled(1, -1, 1);
            for (int i = 0; i < 40; i++)
            {
                krug[i].x = Math.Cos(grad * i) * 13 + 30;
                krug[i].y = 70;
                krug[i].z = Math.Sin(grad * i) * 13 + 70;
            }
            krug = cilinder(krug, 10, 30, 65, 13, 0, "y", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -20, 0, 65, 11, 0, "y", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -45, -25, 70, 9, 0, "y", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -65, -45, 63, 5, 0, "y", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -80, -60, 62, 4, 0, "y", false, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            krug = cilinder(krug, -90, -70, 61, 3, 0, "y", true, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);
            cilinder(krug, -100, -80, 60, 2, 0, "y", true, 0.01f, 0.195f, 0.125f, 0.24f, 0.815f, 0.57f);

            ///ТЕКСТУРА
            Gl.glPushMatrix();
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_BLEND); 
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, mGlTextureObject);        
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);


            Gl.glPushMatrix();
            Gl.glTranslated(100, 50, 70);
            Gl.glScaled(1, -1, 1);
            Gl.glBegin(Gl.GL_QUADS); 
            Gl.glVertex3d(-80, -100, 0);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(-80, 0, 0);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(0, 0, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(0, -100, 0);
            Gl.glTexCoord2f(0, 1);
            Gl.glEnd();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(140, 66, 62);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-100, -125, 0);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(-100, 0, 0);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(0, 0, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(0, -125, 0);
            Gl.glTexCoord2f(0, 1);
            Gl.glEnd();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(0, 90, 67);
            Gl.glRotated(15, 0, 0, 65);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-50, -75, 0);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(-50, 0, 0);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(0, 0, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(0, -75, 0);
            Gl.glTexCoord2f(0, 1);
            Gl.glEnd();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(-45, 80, 65);
            Gl.glRotated(-20, 0, 0, 65);
            Gl.glScaled(1, -1, 1);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-60, -90, 0);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(-60, 0, 0);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(0, 0, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(0, -90, 0);
            Gl.glTexCoord2f(0, 1);
            Gl.glEnd();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(-5, -35, 65);
            Gl.glRotated(-50, 0, 0, 65);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-70, -90, 0);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(-70, 0, 0);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(0, 0, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(0, -90, 0);
            Gl.glTexCoord2f(0, 1);
            Gl.glEnd();
            Gl.glPopMatrix();

            Gl.glDisable(Gl.GL_BLEND);
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glPopMatrix();


            Gl.glPopMatrix();


            //////////////////////////
            ///////////////////////////
            ////////ОКРУЖЕНИЕ/////////
            ////////////////////////
            ////////////////////////  


            //СКАЛЫ

            Gl.glPushMatrix();
            Gl.glTranslated(-60, 30, -1);
            Gl.glScaled(2, 2.5, 5);
            Gl.glColor3f(0.3f, 0.3f, 0.3f);
            Glut.glutSolidDodecahedron();
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2);
            Glut.glutWireDodecahedron();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(-55, 20, -3);
            Gl.glScaled(2, 1.5, 3);
            Gl.glColor3f(0.3f, 0.3f, 0.3f);
            Glut.glutSolidDodecahedron();
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2);
            Glut.glutWireDodecahedron();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(-55, 5, -5);
            Gl.glScaled(4, 3, 3);
            Gl.glColor3f(0.3f, 0.3f, 0.3f);
            Glut.glutSolidDodecahedron();
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2);
            Glut.glutWireDodecahedron();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(-65, 15, -1);
            Gl.glScaled(3, 3, 4);
            Gl.glColor3f(0.3f, 0.3f, 0.3f);
            Glut.glutSolidDodecahedron();
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2);
            Glut.glutWireDodecahedron();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(15, 10, -5);
            Gl.glScaled(4, 3, 1.5);
            Gl.glColor3f(0.3f, 0.3f, 0.3f);
            Glut.glutSolidDodecahedron();
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2);
            Glut.glutWireDodecahedron();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(15, 20, -3);
            Gl.glScaled(2, 2, 3);
            Gl.glColor3f(0.3f, 0.3f, 0.3f);
            Glut.glutSolidDodecahedron();
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2);
            Glut.glutWireDodecahedron();
            Gl.glPopMatrix();




            Gl.glPopMatrix();      
            Gl.glPopMatrix();



            // возвращаем состояние матрицы
            Gl.glPopMatrix();
            // отрисовываем геометрию
            Gl.glFlush();
           
            // обновляем состояние элемента
            AnT.Invalidate();
        }

        private point[] connect(point[] krug, double x2 , double y2, double z2, double R, double rotate, float r, float g , float blue, bool flag)
        {
            int count = 40;
            point[] krug1 = new point[count];
            double grad = Math.PI / count;
            double a,b;
                for (int i = 0; i < count; i++)
                {
                    if (!flag)
                    {
                        a = -Math.Cos(grad * i) * R + z2;
                        b = -Math.Sin(grad * i) * R + y2;
                        krug1[i] = new point(x2, b, a);
                    }
                    else
                    {
                        krug1[i] = new point();
                        a = -Math.Cos(grad * i) * R + x2;
                        b = -Math.Sin(grad * i) * R + z2;
                        krug1[i].x = a;
                        krug1[i].y = y2;
                        krug1[i].z = b;
                    }
                    krug1[i].x = krug1[i].x * Math.Cos(rotate) - krug1[i].y * Math.Sin(rotate);
                    krug1[i].y = krug1[i].x * Math.Sin(rotate) + krug1[i].y * Math.Cos(rotate);
                }
            for(int i = 0; i < count - 1; i++)
            {
                Gl.glColor3f(r, g, blue);
                Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                Gl.glVertex3d(krug[i].x, krug[i].y, krug[i].z);
                Gl.glVertex3d(krug1[i].x, krug1[i].y, krug1[i].z);
                Gl.glVertex3d(krug1[i + 1].x, krug1[i + 1].y, krug1[i + 1].z);
                Gl.glVertex3d(krug[i + 1].x, krug[i + 1].y, krug[i + 1].z);
                Gl.glEnd();
                Gl.glColor3f(0, 0, 0);
                Gl.glBegin(Gl.GL_LINE_STRIP);
                Gl.glVertex3d(krug[i].x, krug[i].y, krug[i].z);
                Gl.glVertex3d(krug[i + 1].x, krug[i + 1].y, krug[i + 1].z);
                Gl.glEnd();
                Gl.glBegin(Gl.GL_LINE_STRIP);
                Gl.glVertex3d(krug1[i].x, krug1[i].y, krug1[i].z);
                Gl.glVertex3d(krug1[i + 1].x, krug1[i + 1].y, krug1[i + 1].z);
                Gl.glEnd();
            }
            return krug1;
        }
      
        private point[] cilinder(point[] circle0, double x1, double y1, double z1, double R1, double rota, string os, bool flag2, float red, float green, float blue, float redOS, float greenOS, float blueOS)
        {
            point[] circle1 = new point[40];
            int count = 40;
            double grad = Math.PI * 2 / count;
            for (int i =0;i<count; i++)
            {
               
                circle1[i] = new point();

                if (os.Equals("z"))
                {
                    circle1[i].x = Math.Cos(grad * i) * R1 + x1; 
                    circle1[i].y = Math.Sin(grad * i) * R1 + y1;
                    circle1[i].z = z1;
                }
                if (os.Equals("y"))
                {
                    circle1[i].x = Math.Cos(grad * i) * R1 + x1;
                    circle1[i].y = y1;
                    circle1[i].z = Math.Sin(grad * i) * R1 + z1;
                }
                if (os.Equals("x"))
                {
                    circle1[i].x = x1;
                    circle1[i].y = Math.Sin(grad * i) * R1 + y1;
                    circle1[i].z = Math.Cos(grad * i) * R1 + z1;
                }
                if (rota != 0)
                {
                    circle1[i].y = circle1[i].y - y1;
                    circle1[i].z = circle1[i].z - z1;
                    circle1[i].y = circle1[i].y*Math.Cos((rota*Math.PI)/180)- circle1[i].z*Math.Sin((rota*Math.PI)/180);
                    circle1[i].z = circle1[i].y * Math.Sin((rota * Math.PI) / 180) + circle1[i].z * Math.Cos((rota * Math.PI) / 180);
                    circle1[i].y = circle1[i].y + y1;
                    circle1[i].z = circle1[i].z + z1;
                }
            }
            Gl.glColor3f(red, green, blue);
            Gl.glLineWidth(5f);
            for (int i = 0; i < count-1; i++)
            {
                    Gl.glColor3f(red, green, blue);
                    Gl.glBegin(Gl.GL_TRIANGLE_FAN);
                    Gl.glVertex3d(circle0[i].x, circle0[i].y, circle0[i].z);
                    Gl.glVertex3d(circle1[i].x, circle1[i].y, circle1[i].z);
                    Gl.glVertex3d(circle1[i+1].x, circle1[i+1].y, circle1[i+1].z);
                    Gl.glVertex3d(circle0[i + 1].x, circle0[i + 1].y, circle0[i + 1].z);
                    Gl.glEnd();
                    Gl.glColor3f(redOS, greenOS, blueOS);
                    if (!flag2)
                    {
                        Gl.glBegin(Gl.GL_LINE_STRIP);
                        Gl.glVertex3d(circle0[i].x, circle0[i].y, circle0[i].z);
                        Gl.glVertex3d(circle0[i + 1].x, circle0[i + 1].y, circle0[i + 1].z);
                        Gl.glEnd();
                        Gl.glBegin(Gl.GL_LINE_STRIP);
                        Gl.glVertex3d(circle1[i].x, circle1[i].y, circle1[i].z);
                        Gl.glVertex3d(circle1[i + 1].x, circle1[i + 1].y, circle1[i + 1].z);
                        Gl.glEnd();
                    }                  
            }
            Gl.glColor3f(red, green, blue);
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glVertex3d(circle0[count-1].x, circle0[count-1].y, circle0[count-1].z);
            Gl.glVertex3d(circle1[count-1].x, circle1[count-1].y, circle1[count-1].z);
            Gl.glVertex3d(circle1[0].x, circle1[0].y, circle1[0].z);
            Gl.glVertex3d(circle0[0].x, circle0[0].y, circle0[0].z);
            Gl.glEnd();

            if (!flag2)
            {
                Gl.glColor3f(redOS, greenOS, blueOS);
                Gl.glBegin(Gl.GL_LINE_STRIP);
                Gl.glVertex3d(circle0[count - 1].x, circle0[count - 1].y, circle0[count - 1].z);
                Gl.glVertex3d(circle0[0].x, circle0[0].y, circle0[0].z);
                Gl.glEnd();
                Gl.glBegin(Gl.GL_LINE_STRIP);
                Gl.glVertex3d(circle1[count - 1].x, circle1[count - 1].y, circle1[count - 1].z);
                Gl.glVertex3d(circle1[0].x, circle1[0].y, circle1[0].z);
                Gl.glEnd();
            }
            return circle1;
        }
        private void fullcircle(double x, double y, double z, double R, int count, float r, float g, float blue)
        {
            point[] krug = new point[count];
            double grad = Math.PI*2 / count;
            double a, b;
            Gl.glColor3f(r, g, blue);
            for (int i = 0; i < count; i++)
            {                   
                    Gl.glBegin(Gl.GL_TRIANGLES);
                    Gl.glVertex3d(x, y, z);
                    a = -Math.Cos(grad * i) * R + z;
                    b = -Math.Sin(grad * i) * R + y;
                    krug[i] = new point(x, b, a);
                    Gl.glVertex3d(x, b, a);
                    a = -Math.Cos(grad * (i+1)) * R + z;
                    b = -Math.Sin(grad * (i+1)) * R + y;
                    Gl.glVertex3d(x, b, a);
                Gl.glEnd();
            }
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(3f);
            for (int i = 0; i < count-1; i++)
            {
                Gl.glBegin(Gl.GL_LINE_STRIP);
                Gl.glVertex3d(krug[i].x, krug[i].y, krug[i].z);
                Gl.glVertex3d(krug[i+1].x, krug[i+1].y, krug[i+1].z);
                Gl.glEnd();
            }
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3d(krug[0].x, krug[0].y, krug[0].z);
            Gl.glVertex3d(krug[39].x, krug[39].y, krug[39].z);
            Gl.glEnd();          
        }
            private point[] circle(double x, double y, double z, double R, string os, double grad, bool ba, bool bb, int count, float width)
        {
            point[] krug = new point[count];
            grad = grad / count;
            double a, b;
            Gl.glLineWidth(width);
            Gl.glColor3f(0, 0, 0);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            if (os.Equals("x"))
            {
                for (int i = 0; i < count; i++)
                {
                    if(ba)
                        a = Math.Cos(grad * i) * R + z;
                    else
                        a = -Math.Cos(grad * i) * R + z;
                    if(bb)
                        b = Math.Sin(grad * i) * R + y;
                    else
                        b = -Math.Sin(grad * i) * R + y;
                    krug[i] = new point(x, b, a);
                    Gl.glVertex3d(x, b, a);
                }
            }
            if (os.Equals("y"))
            {
                for (int i = 0; i < count; i++)
                {
                    if(ba)
                        a = Math.Cos(grad * i) * R + x;
                    else
                        a = -Math.Cos(grad * i) * R + x;
                    if(bb)
                        b = Math.Sin(grad * i) * R + z;
                    else
                        b = -Math.Sin(grad * i) * R + z;
                    krug[i] = new point(a, y, b);
                    Gl.glVertex3d(a, y, b);
                }
            }
            if (os.Equals("z"))
            {
                for (int i = 0; i < count; i++)
                {
                    if(ba)
                        a = Math.Cos(grad * i) * R + x;
                    else
                        a = -Math.Cos(grad * i) * R + x;
                    if(bb)
                        b = Math.Sin(grad * i) * R + y;
                    else
                        b = -Math.Sin(grad * i) * R + y;
                    krug[i] = new point(a, b, z);
                    Gl.glVertex3d(a, b, z);
                }
            }
            Gl.glEnd();
            return krug;

        }

        private void block(double x, double y, double z, float r, float g, float b, float lineWidth)
        {
            Gl.glColor3f(r, g, b);


            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glVertex3d(x+6, y, z);
            Gl.glVertex3d(x, y, z);
            Gl.glVertex3d(x, y-2, z);
            Gl.glVertex3d(x+6, y-2, z);
            Gl.glEnd();
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glVertex3d(x+6, y, z+0.5);
            Gl.glVertex3d(x+6, y, z);
            Gl.glVertex3d(x + 6, y-2, z);
            Gl.glVertex3d(x + 6, y-2, z+0.5);
            Gl.glEnd();
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);

            Gl.glVertex3d(x + 6, y, z+0.5);
            Gl.glVertex3d(x, y, z+0.5);
            Gl.glVertex3d(x, y-2, z+0.5);
            Gl.glVertex3d(x + 6, y-2, z+0.5);
            Gl.glEnd();
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glVertex3d(x, y, z+0.5);
            Gl.glVertex3d(x, y, z);
            Gl.glVertex3d(x, y-2, z);
            Gl.glVertex3d(x, y-2, z+0.5);
            Gl.glEnd();
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glVertex3d(x + 6, y, z);
            Gl.glVertex3d(x, y, z);
            Gl.glVertex3d(x, y, z+0.5);
            Gl.glVertex3d(x + 6, y, z+0.5);
            Gl.glEnd();
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glVertex3d(x + 6, y-2, z);
            Gl.glVertex3d(x, y-2, z);
            Gl.glVertex3d(x, y-2, z+0.5);
            Gl.glVertex3d(x + 6, y-2, z+0.5);
            Gl.glEnd();

            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(lineWidth);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3d(x + 6, y, z);
            Gl.glVertex3d(x, y, z);
            Gl.glVertex3d(x, y, z+0.5);
            Gl.glVertex3d(x + 6, y, z+0.5);
            Gl.glVertex3d(x + 6, y, z);
            Gl.glVertex3d(x + 6, y-2, z);
            Gl.glVertex3d(x, y-2, z);
            Gl.glVertex3d(x, y, z);
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3d(x + 6, y-2, z+0.5);
            Gl.glVertex3d(x, y-2, z+0.5);
            Gl.glVertex3d(x, y-2, z);         
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3d(x, y - 2, z + 0.5);
            Gl.glVertex3d(x, y , z + 0.5);
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3d(x+6, y , z + 0.5);
            Gl.glVertex3d(x + 6, y-2, z + 0.5);
            Gl.glVertex3d(x + 6, y - 2, z);
            Gl.glEnd();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // устанавливаем фокус в AnT
            AnT.Focus();
        }

        private static uint MakeGlTexture(int Format, IntPtr pixels, int w, int h)
        {

            // идентификатор текстурного объекта
            uint texObject;

            // генерируем текстурный объект
            Gl.glGenTextures(1, out texObject);

            // устанавливаем режим упаковки пикселей
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);

            // создаем привязку к только что созданной текстуре
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texObject);

            // устанавливаем режим фильтрации и повторения текстуры
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);

            // создаем RGB или RGBA текстуру
            switch (Format)
            {

                case Gl.GL_RGB:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, w, h, 0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;

                case Gl.GL_RGBA:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, w, h, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;

            }

            // возвращаем идентификатор текстурного объекта

            return texObject;

        }
    }
}
