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
        Level.Height = 2000;
        Level.Width = 2000;
        Level.CreateBorders();
        Level.BackgroundColor = Color.AshGray;
        Gravity = new Vector(0, -500);
        // PhysicsStructure pelaaja1
        LuoPelihahmo(this, 0, Level.Bottom + 200);
        //PhysicsObject pelaaja = new PhysicsObject(100, 100, Shape.Hexagon);
        //Add(pelaaja);
        Camera.ZoomToAllObjects();
        // luo aliohjelma tikku-ukon luomiseen


        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    public static void LuoPelihahmo(PhysicsGame game, double x, double y)
    {
        PhysicsObject la1, la2, ra1, ra2, h, b, ll1, ll2, rl1, rl2;
        double sivu = 200;
        // luo pelihahmon osat aliohjemalla, liitä osat yhteen parametrina tulee ukon jalkojen välissä olevan pisteen paikka pelikentällä, jonka suhteen muut palikat luodaan
        ll2 = LuoOsa(game, x - ((1.5 * sivu) / Math.Sqrt(2.0)), y, sivu, 0.25 * sivu, -45d);
        rl2 = LuoOsa(game, x + ((1.5 * sivu) / Math.Sqrt(2.0)), y, sivu, 0.25 * sivu, 45d);
        ll1 = LuoOsa(game, x - ((0.5 * sivu) / Math.Sqrt(2.0)), y + (sivu / Math.Sqrt(2.0)), sivu, 0.25 * sivu, -45d);
        rl1 = LuoOsa(game, x + ((0.5 * sivu) / Math.Sqrt(2.0)), y + (sivu / Math.Sqrt(2.0)), sivu, 0.25 * sivu, 45d);
        b = LuoOsa(game, x, y + ((1.5 * sivu) / Math.Sqrt(2.0)) + sivu, 2 * sivu, 0.25 * sivu, 0d);
        la1 = LuoOsa(game, b.X - 0.5 * sivu, b.Y + 0.885 * sivu, sivu, 0.25 * sivu, 90d);
        la2 = LuoOsa(game, la1.X - sivu, la1.Y, sivu, 0.25 * sivu, 90d);
        ra1 = LuoOsa(game, b.X + 0.5 * sivu, b.Y + 0.885 * sivu, sivu, 0.25 * sivu, 90d);
        ra2 = LuoOsa(game, ra1.X + sivu, ra1.Y, sivu, 0.25 * sivu, 90d);
        h = LuoPaa(game, x, b.Y + 1.5 * sivu, 0.5 * sivu);

        PhysicsStructure pelihahmo = new PhysicsStructure(ll1, ll2, rl1, rl2, b, h, ra1, ra2, la1, la2);
        pelihahmo.Softness = 50;
        game.Add(pelihahmo);
    }

    public static PhysicsObject LuoOsa(PhysicsGame game, double x, double y, double height, double width, double angle)
    {
        //Luo suorakulmion muotoisen rakenneosan, parametreinä pituus voi vaihdella
        PhysicsObject osa = new PhysicsObject(width, height, Shape.Rectangle, x, y);
        osa.Angle = Angle.FromDegrees(angle);
        game.Add(osa);
        return osa;
    }

    public static PhysicsObject LuoPaa(PhysicsGame game, double x, double y, double r)
    {
        //luo pää tikku-ukolle
        PhysicsObject head = new PhysicsObject(2 * r, 2 * r, Shape.Circle, x, y);
        game.Add(head);
        return head;
    }
}
