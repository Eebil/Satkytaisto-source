using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class Satkytaisto : PhysicsGame
{
    DoubleMeter pelaaja1Hitpoints;
    DoubleMeter pelaaja2Hitpoints;

    SoundEffect bodyShot = LoadSoundEffect("hit");
    SoundEffect finisher = LoadSoundEffect("finisher");
    SoundEffect block = LoadSoundEffect("block");
    SoundEffect headShot = LoadSoundEffect("headshot");


    public override void Begin()
    {
        IsFullScreen = true;
        Level.Height = 4000;
        Level.Width = 4000;
        Level.CreateBorders();
        Level.BackgroundColor = Color.Black;
        Gravity = new Vector(0, -700);
        MediaPlayer.Play("XDerpacito");
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.3;

        LuoPuu(this, 0, Level.Bottom + 500, 1000, 20, 0.0);
        // PhysicsStructure pelaaja1
        List<PhysicsObject> p1Objects = LuoPelihahmo(this, 500, Level.Bottom + 200, Color.HotPink, "pelaaja1");
        List<PhysicsObject> p2Objects = LuoPelihahmo(this, -500, Level.Bottom + 200, Color.Gold, "pelaaja2");

        PhysicsObject pelaaja1 = p1Objects[9];
        PhysicsObject pelaaja1Ra = p1Objects[7];
        PhysicsObject pelaaja1La = p1Objects[5];

        PhysicsObject pelaaja2 = p2Objects[9];
        PhysicsObject pelaaja2Ra = p2Objects[7];
        PhysicsObject pelaaja2La = p2Objects[5];



        //Camera.ZoomToAllObjects();

        Camera.ZoomFactor = 0.5;          
        Camera.Follow(pelaaja1, pelaaja2);

        pelaaja1Hitpoints = CreateHealthbar(pelaaja1Hitpoints, Screen.Right - 200, Screen.Top - 20, "Player 1 Wins");
        pelaaja2Hitpoints = CreateHealthbar(pelaaja2Hitpoints, Screen.Left + 200, Screen.Top - 20, "Player 2 Wins");



        Keyboard.Listen(Key.Left, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja1 vasemmalle", pelaaja1, new Vector(-5000, 0));
        Keyboard.Listen(Key.Right, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja1 oikealle", pelaaja1, new Vector(5000, 0));
        Keyboard.Listen(Key.Up, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja1 ylös", pelaaja1, new Vector(0, 5000));
        Keyboard.Listen(Key.Down, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja1 alas", pelaaja1, new Vector(0, -5000));

        Keyboard.Listen(Key.RightShift, ButtonState.Pressed, HeilautaKasia, "heilauta pelaaja1 käsia ylös", pelaaja1Ra, pelaaja1La, new Vector(0, 1000));
        Keyboard.Listen(Key.RightControl, ButtonState.Pressed, HeilautaKasia, "heilauta pelaaja1 käsia alas", pelaaja1Ra, pelaaja1La, new Vector(0, -1000));

        Keyboard.Listen(Key.A, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja2 vasemmalle", pelaaja2, new Vector(-5000, 0));
        Keyboard.Listen(Key.D, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja2 oikealle", pelaaja2, new Vector(5000, 0));
        Keyboard.Listen(Key.W, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja2 ylös", pelaaja2, new Vector(0, 5000));
        Keyboard.Listen(Key.S, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja2 alas", pelaaja2, new Vector(0, -5000));

        Keyboard.Listen(Key.G, ButtonState.Pressed, HeilautaKasia, "heilauta pelaaja2 käsia ylös", pelaaja2Ra, pelaaja2La, new Vector(0, 1000));
        Keyboard.Listen(Key.V, ButtonState.Pressed, HeilautaKasia, "heilauta pelaaja2 käsia alas", pelaaja2Ra, pelaaja2La, new Vector(0, -1000));

        LisaaTormayskasittelija(p1Objects, "pelaaja1Ase", "pelaaja1Head", "pelaaja2Body", "pelaaja2Head", "pelaaja2Ase");
        LisaaTormayskasittelija(p2Objects, "pelaaja2Ase", "pelaaja2Head", "pelaaja1Body", "pelaaja1Head", "pelaaja1Ase");




        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    private static void LuoPuu(PhysicsGame game, double x, double y, double height, double width, double suunta)
    {
        GameObject puu = new GameObject(width, height, Shape.Rectangle, x, y);
        puu.Color = Color.Brown;
        puu.Angle = Angle.FromRadians(suunta);
        game.Add(puu);

        if (height < 50) return;

        double uheight = height * 0.8;
        double uwidth = width * 0.95;
        double osuunta = suunta - Math.PI / 5;
        double vsuunta = suunta + Math.PI / 5;

        LuoPuu(game, x + Math.Cos(osuunta) * ((height / 2) + (uheight / 2)), y + Math.Sin(osuunta) * ((height / 2) + (uheight / 2)), uheight, uwidth, osuunta);
        
    }

    private DoubleMeter CreateHealthbar(DoubleMeter hitpoints, double x, double y, string message)
    {
        hitpoints = new DoubleMeter(100);
        hitpoints.MaxValue = 100;
        //hitpoints.LowerLimit += ZeroHp(message);

        ProgressBar hpBar = new ProgressBar(300, 20);
        hpBar.X = x;
        hpBar.Y = y;
        hpBar.BindTo(hitpoints);
        Add(hpBar);

        return hitpoints;
    }


    private void LisaaTormayskasittelija(List<PhysicsObject> osat, string hitterAse, string hitterHead, string targetBody, string targetHead, string targetAse)
    {
        //TODO: (Ase--> body ..... Head ----> body) (Ase--->Head .....Ase---->Ase)  (Head-->Head)
        foreach (PhysicsObject osa in osat)
        {
            if (osa.Tag.ToString() == hitterAse || osa.Tag.ToString() == hitterHead)
            {
                AddCollisionHandler(osa, targetBody, BodyShot);
                AddCollisionHandler(osa, targetHead, HeadShot);
                AddCollisionHandler(osa, targetAse, BlockShot);
            }
        }
    }

    private void BlockShot(IPhysicsObject hitter, IPhysicsObject target)
    {
        Explosion osuma = new Explosion(100);
        osuma.Position = target.Position;
        osuma.Speed = 700;
        osuma.Force = 300;
        osuma.Image = null;
        osuma.Sound = block;
        Add(osuma); 
    }

    private void HeadShot(IPhysicsObject hitter, IPhysicsObject target)
    {
        if (target.Tag.ToString() == "pelaaja2Head")
        {
            pelaaja2Hitpoints.Value -= 10;
           // pelaaja2Hitpoints -= 10;
            if (pelaaja2Hitpoints.Value <= 0)
            {
                target.Destroy();
                finisher.Play();
                Camera.ZoomToLevel();
                //ClearControls();
                //TODO PELAAJA 1 VOITTI
            }
        }

        if (target.Tag.ToString() == "pelaaja1Head")
        {
            pelaaja1Hitpoints.Value -= 10;
           // pelaaja1Hitpoints -= 10;
            if (pelaaja1Hitpoints.Value <= 0)
            {
                target.Destroy();
                finisher.Play();
                Camera.ZoomToLevel();
                //ClearControls();
                //TODO PELAAJA 2 VOITTI
            }
        }

        Explosion osuma = new Explosion(100);
        osuma.Position = target.Position;
        osuma.Speed = 700;
        osuma.Force = 300;
        osuma.Image = null;
        osuma.Color = Color.Red;
        osuma.Sound = headShot;
        Add(osuma);
    }


    private void BodyShot(PhysicsObject hitter, PhysicsObject target)
    {
        if (target.Tag.ToString() == "pelaaja2Body")
        {
            pelaaja2Hitpoints.Value -= 5;
           // pelaaja2Hitpoints -= 5;
            if (pelaaja2Hitpoints.Value <= 0)
            {
                target.Destroy();
                finisher.Play();
            }
        }
        // Tässä toistoa, mutta en osaa antaa lisää parametreja tuolta collision handlereista
        if (target.Tag.ToString() == "pelaaja1Body")
        {
            pelaaja1Hitpoints.Value -= 5;
           // pelaaja1Hitpoints -= 5;
            if (pelaaja1Hitpoints.Value <= 0)
            {
                target.Destroy();
                finisher.Play();
            }
        }


        Explosion osuma = new Explosion(100);
        osuma.Position = target.Position;
        osuma.Speed = 700;
        osuma.Force = 300;
        osuma.Sound = bodyShot;
        osuma.Image = null;
        osuma.Color = Color.Yellow;
        Add(osuma);


    }

    private void HeilautaKasia(PhysicsObject ra, PhysicsObject la, Vector suunta)
    {
        ra.Hit(suunta);
        la.Hit(suunta);
    }


    /// <summary>
    /// Aliohjelmalla luodaan kentälle pelihahmo, joka koostuu suorakulmion muotoisista komponenteista sekä ympyrän muotoisesta päästä
    /// </summary>
    /// <param peli="game"></param>
    /// <param X-koordinaatti="x"></param>
    /// <param Y-koordinaatti="y"></param>
    /// <param hahmon väri="color"></param>
    /// <param hahmon tägi="tag"></param>
    /// <returns></returns>
    private static List<PhysicsObject> LuoPelihahmo(PhysicsGame game, double x, double y, Color color, string tag)
    {
        PhysicsObject la1, la2, ra1, ra2, h, b, ll1, ll2, rl1, rl2;
        double sivu = 100;
        // luo pelihahmon osat aliohjemalla, liitä osat yhteen parametrina tulee ukon jalkojen välissä olevan pisteen paikka pelikentällä, jonka suhteen muut palikat luodaan DONE

        // Nämä osat muodostavat pelihahmot haavoittuvat osat
        ll1 = LuoOsa(game, x - ((0.5 * sivu) / Math.Sqrt(2.0)), y + (sivu / Math.Sqrt(2.0)), sivu, 0.25 * sivu, -45d, color, tag + "Body");
        rl1 = LuoOsa(game, x + ((0.5 * sivu) / Math.Sqrt(2.0)), y + (sivu / Math.Sqrt(2.0)), sivu, 0.25 * sivu, 45d, color, tag + "Body");
        b = LuoOsa(game, x, y + ((1.5 * sivu) / Math.Sqrt(2.0)) + sivu, 2 * sivu, 0.25 * sivu, 0d, color, tag + "Body");
        la1 = LuoOsa(game, b.X - 0.5 * sivu, b.Y + 0.885 * sivu, sivu, 0.25 * sivu, 90d, color, tag + "Body");
        ra1 = LuoOsa(game, b.X + 0.5 * sivu, b.Y + 0.885 * sivu, sivu, 0.25 * sivu, 90d, color, tag + "Body");

        //päähän osumisesta lisää damagea
        h = LuoPaa(game, x, b.Y + 1.5 * sivu, 0.5 * sivu, color, tag + "Head");

        // Nämä osat tekevät vahinkoa osuessaan vartaloon/päähän
        ll2 = LuoOsa(game, x - ((1.5 * sivu) / Math.Sqrt(2.0)), y, sivu, 0.25 * sivu, -45d, color, tag + "Ase");
        rl2 = LuoOsa(game, x + ((1.5 * sivu) / Math.Sqrt(2.0)), y, sivu, 0.25 * sivu, 45d, color, tag + "Ase");
        la2 = LuoOsa(game, la1.X - sivu, la1.Y, sivu, 0.25 * sivu, 90d, color, tag + "Ase");
        ra2 = LuoOsa(game, ra1.X + sivu, ra1.Y, sivu, 0.25 * sivu, 90d, color, tag + "Ase");

        List<PhysicsObject> objects = new List<PhysicsObject>() { ll1, ll2, rl1, rl2, la1, la2, ra1, ra2, b, h };

        // Muuta aliohjelmaksi ja korvaa kaikki liitokset axlejointeilla. DONE
        AxleJoint rightElbow = LuoLiitos(game, ra1, ra2, ra1.X + (0.5 * sivu), ra1.Y);
        AxleJoint leftElbow = LuoLiitos(game, la1, la2, la1.X - (0.5 * sivu), la1.Y);
        AxleJoint leftShoulder = LuoLiitos(game, la1, b, la1.X + (0.5 * sivu), la1.Y);
        AxleJoint rightShoulder = LuoLiitos(game, ra1, b, ra1.X - (0.5 * sivu), la1.Y);
        AxleJoint rightKnee = LuoLiitos(game, rl1, rl2, (rl1.X + rl2.X) / 2, (rl1.Y + rl2.Y) / 2);
        AxleJoint leftKnee = LuoLiitos(game, ll1, ll2, (ll1.X + ll2.X) / 2, (ll1.Y + ll2.Y) / 2);
        AxleJoint rightHip = LuoLiitos(game, ll1, b, x, y + ((1.5 * sivu) / Math.Sqrt(2.0)));
        AxleJoint leftHip = LuoLiitos(game, rl1, b, x, y + ((1.5 * sivu) / Math.Sqrt(2.0)));
        AxleJoint neck = LuoLiitos(game, h, b, x, b.Y + sivu);

        // Tee Lista osista ja palauta kaikki pääohjelman käyttöön
        return objects;
    }


    /// <summary>
    /// Aliohjelmalla luodaan pelihahmon rakenneosana toimiva suorakulma annettujen parametrien avulla
    /// </summary>
    /// <param peli="game"></param>
    /// <param X-koordinaatti="x"></param>
    /// <param Y-koordinaatti="y"></param>
    /// <param osanKorkeus="height"></param>
    /// <param osanLeveys="width"></param>
    /// <param osanKulma="angle"></param>
    /// <param osanVäri="color"></param>
    /// <param osanTägi="tag"></param>
    /// <returns></returns>
    private static PhysicsObject LuoOsa(PhysicsGame game, double x, double y, double height, double width, double angle, Color color, string tag)
    {
        //Luo suorakulmion muotoisen rakenneosan, parametreinä pituus voi vaihdella
        PhysicsObject osa = new PhysicsObject(width, height, Shape.Rectangle, x, y);
        osa.Angle = Angle.FromDegrees(angle);
        osa.Color = color;
        //osa.LinearDamping = 0.999;
        osa.Restitution = 0.7;
        osa.Tag = tag;
        game.Add(osa);

        if (osa.Tag.ToString() == "pelaaja1Body") { osa.CollisionIgnoreGroup = 1; }
        if (osa.Tag.ToString() == "pelaaja2Body") { osa.CollisionIgnoreGroup = 2; }
        return osa;
    }


    /// <summary>
    /// Aliohjelma luo pään hahmolle
    /// </summary>
    /// <param peli="game"></param>
    /// <param X-koordinaatti="x"></param>
    /// <param Y-koordinaatti="y"></param>
    /// <param päänSäde="r"></param>
    /// <param päänVäri="color"></param>
    /// <param päänTägi="tag"></param>
    /// <returns></returns>
    private static PhysicsObject LuoPaa(PhysicsGame game, double x, double y, double r, Color color, string tag)
    {
        //luo pää tikku-ukolle
        PhysicsObject head = new PhysicsObject(2 * r, 2 * r, Shape.Circle, x, y);
        head.Color = color;
        head.Restitution = 0.5;
        head.Tag = tag;
        game.Add(head);
        return head;
    }


    /// <summary>
    /// Aliohjelma luo nivel-liitoken kahden osan välille
    /// </summary>
    /// <param peli="game"></param>
    /// <param sidottavaOsa1="osa1"></param>
    /// <param sidottavaOsa2="osa2"></param>
    /// <param nivelenX-koordinaatti="x"></param>
    /// <param nivelenY-koordinaatti="y"></param>
    /// <returns></returns>
    private static AxleJoint LuoLiitos(PhysicsGame game, PhysicsObject osa1, PhysicsObject osa2, double x, double y)
    {
        AxleJoint liitos = new AxleJoint(osa1, osa2, new Vector(x, y));
        liitos.Softness = 0.01;
        game.Add(liitos);
        return liitos;
    }


    private void LiikutaPelaajaa(PhysicsObject pelaaja, Vector suunta)
    {
        pelaaja.Push(suunta);
    }
}
