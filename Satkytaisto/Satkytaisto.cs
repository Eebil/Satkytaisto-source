using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class Satkytaisto : PhysicsGame
{
    public override void Begin()
    {
        Level.Height = 4000;
        Level.Width = 4000;
        Level.CreateBorders();
        Level.BackgroundColor = Color.Black;
        Gravity = new Vector(0, -500);
        // PhysicsStructure pelaaja1
        List<PhysicsObject> p1Objects = LuoPelihahmo(this, 500, Level.Bottom + 200, Color.Mint, "pelaaja1");
        List<PhysicsObject> p2Objects = LuoPelihahmo(this, -500, Level.Bottom + 200, Color.Blue, "pelaaja2");

        PhysicsObject pelaaja1 = p1Objects[9];
        PhysicsObject pelaaja1Ra = p1Objects[7];
        PhysicsObject pelaaja1La = p1Objects[5];

        PhysicsObject pelaaja2 = p2Objects[9];
        PhysicsObject pelaaja2Ra = p2Objects[7];
        PhysicsObject pelaaja2La = p2Objects[5];

        //7foreach (PhysicsObject osa in p1Objects)
        //{
        //    osa.CollisionIgnoreGroup = 1;
        //}

        //Camera.ZoomToAllObjects();

        Camera.ZoomFactor = 0.5;          //TODO: kamera pehmeämmäksi tässä ratkaisussa
        Camera.Follow(pelaaja1, pelaaja2);

        // luo aliohjelma tikku-ukon luomiseen


        Keyboard.Listen(Key.Left, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja1 vasemmalle", pelaaja1, new Vector(-5000, 0));
        Keyboard.Listen(Key.Right, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja1 oikealle", pelaaja1, new Vector(5000, 0));
        Keyboard.Listen(Key.Up, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja1 ylös", pelaaja1, new Vector(0, 5000));
        Keyboard.Listen(Key.Down, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja1 alas", pelaaja1, new Vector(0, -5000));

        //saisiko kädet heilahtamaan vartalon suuntaisesti?
        Keyboard.Listen(Key.RightShift, ButtonState.Pressed, HeilautaKasia, "heilauta pelaaja1 käsia ylös", pelaaja1Ra, pelaaja1La, new Vector(0, 1000));
        Keyboard.Listen(Key.RightControl, ButtonState.Pressed, HeilautaKasia, "heilauta pelaaja1 käsia alas", pelaaja1Ra, pelaaja1La, new Vector(0, -1000));

        Keyboard.Listen(Key.A, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja2 vasemmalle", pelaaja2, new Vector(-5000, 0));
        Keyboard.Listen(Key.D, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja2 oikealle", pelaaja2, new Vector(5000, 0));
        Keyboard.Listen(Key.W, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja2 ylös", pelaaja2, new Vector(0, 5000));
        Keyboard.Listen(Key.S, ButtonState.Down, LiikutaPelaajaa, "liikuta pelaaja2 alas", pelaaja2, new Vector(0, -5000));

        Keyboard.Listen(Key.G, ButtonState.Pressed, HeilautaKasia, "heilauta pelaaja2 käsia ylös", pelaaja2Ra, pelaaja2La, new Vector(0, 1000));
        Keyboard.Listen(Key.V, ButtonState.Pressed, HeilautaKasia, "heilauta pelaaja2 käsia alas", pelaaja2Ra, pelaaja2La, new Vector(0, -1000));

        //TODO: Törmäykset  SOLVED, Lisää kaikki muutkin handlerit
        foreach (PhysicsObject osa in p1Objects)
        {
            if (osa.Tag.ToString() == "pelaaja1Ase")
            {
                AddCollisionHandler(osa, "pelaaja2Body", Pelaaja1Osuu);
            }
        }
        


        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    private void Pelaaja1Osuu(PhysicsObject pelaaja1Ase, PhysicsObject pelaaja2Body)
    {
        //TODO: Pelaaja2Health--
        Explosion osuma = new Explosion(200);
        osuma.Position = pelaaja2Body.Position;
        osuma.Speed = 1000;
        osuma.Force = 1000;
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
    public static List<PhysicsObject> LuoPelihahmo(PhysicsGame game, double x, double y, Color color, string tag)
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
    public static PhysicsObject LuoOsa(PhysicsGame game, double x, double y, double height, double width, double angle, Color color, string tag)
    {
        //Luo suorakulmion muotoisen rakenneosan, parametreinä pituus voi vaihdella
        PhysicsObject osa = new PhysicsObject(width, height, Shape.Rectangle, x, y);
        osa.Angle = Angle.FromDegrees(angle);
        osa.Color = color;
        osa.Tag = tag;
        game.Add(osa);
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
    public static PhysicsObject LuoPaa(PhysicsGame game, double x, double y, double r, Color color, string tag)
    {
        //luo pää tikku-ukolle
        PhysicsObject head = new PhysicsObject(2 * r, 2 * r, Shape.Circle, x, y);
        head.Color = color;
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
    public static AxleJoint LuoLiitos(PhysicsGame game, PhysicsObject osa1, PhysicsObject osa2, double x, double y)
    {
        AxleJoint liitos = new AxleJoint(osa1, osa2, new Vector(x, y));
        liitos.Softness = 0.05;
        game.Add(liitos);
        return liitos;
    }


    public void LiikutaPelaajaa(PhysicsObject pelaaja, Vector suunta)
    {
        pelaaja.Push(suunta);
    }
}
