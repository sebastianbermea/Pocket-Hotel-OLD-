using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Translate : MonoBehaviour
{
    public bool tmt;
    Text txt;
    TextMeshPro tmxt;
    public TextMeshProUGUI tmtxUI;
    [TextArea]
    public string eng, esp;
    // Start is called before the first frame update
    void Start()
    {
        if (tmtxUI)
        {
            tmtxUI.text = SetText();
            return;
        }
        if (!tmt)
            txt = GetComponent<Text>();
        else
            tmxt = GetComponent<TextMeshPro>();
        if (txt == null && tmxt == null)
            return;

        string temp = SetText();
       
        if (txt != null)
            txt.text = temp;
        else if (tmxt != null)
            tmxt.text = temp;
    }
    public string SetText()
    {
        if (GC.INS)
        {
            switch (GC.INS.idiom)
            {
                default:
                    return eng;
                case 0:
                    return eng;
                case 1:
                    return esp;
            }
        }
        else
        {
            switch (Application.systemLanguage)
            {
                default:
                    return eng;
                case SystemLanguage.English:
                    return eng;
                case SystemLanguage.Spanish:
                    return esp;
            }

        }
       

    }
    public string GetText(int x)
    {
        switch (GC.INS.idiom)
        {
            default:
                return English(x);
            case 0:
                return English(x);
            case 1:
                return Spanish(x);
        }
    }
    string English(int x)
    {
        switch (x)
        {
            default:
                return "";
            case 0:
                return " blocks used";
            case 1:
                return "You've unlocked: ";
            case 2:
                return "Next level is ";
            case 3:
                return " points away and will unlock: ";
            case 4:
                return " and ";
            case 5:
                return "24 blocks expand";
            case 6:
                return "28 blocks expand";
            case 7:
                return "34 blocks expand";
            case 8:
                return "42 blocks expand";
            case 9:
                return "52 blocks expand";
            case 10:
                return "64 blocks expand";
            case 11:
                return "76 blocks expand";
            case 12:
                return "92 blocks expand";
            case 13:
                return "116 blocks expand";
            case 14:
                return "128 blocks expand";
            case 15:
                return "144 blocks expand";
            case 16:
                return "164 blocks expand";
            case 17:
                return "176 blocks expand";
            case 18:
                return "194 blocks expand";
            case 19:
                return "210 blocks expand";
            case 20:
                return "224 blocks expand";
            case 21:
                return "256 blocks expand";
            case 22:
                return "272 blocks expand";
            case 23:
                return "312 blocks expand";
            case 24:
                return " points away";
            case 25:
                return " Outfit";
            case 26:
                return "hour";
            case 27:
                return "Fix ";
            case 28:
                return "Clean ";
            case 29:
                return " room";
            case 30:
                return " rooms";
            case 31:
                return "Answer ";
            case 32:
                return " room calls";
            case 33:
                return " pipes";
            case 34:
                return "Find ";
            case 35:
                return " keys";
            case 36:
                return " room energies";
            case 37:
                return "Click on ";
            case 38:
                return " costumer tips";
            case 39:
                return "Visit ";
            case 40:
                return " friends";
            case 41:
                return " friend";
            case 42:
                return "Purchase ";
            case 43:
                return " decorations";
            case 44:
                return " decoration";
            case 45:
                return " room paints";
            case 46:
                return " room paint";
            case 47:
                return " room floors";
            case 48:
                return " room floor";
            case 49:
                return " room beds";
            case 50:
                return " room bed";
            case 51:
                return " wall objects";
            case 52:
                return " wall object";
            case 53:
                return " floor objects";
            case 54:
                return " floor object";
            case 55:
                return " ceiling objects";
            case 56:
                return " ceiling object";
            case 57:
                return "Pop ";
            case 58:
                return " clouds";
            case 59:
                return " reward clouds";
            case 60:
                return " staff outfits";
            case 61:
                return " staff outfit";
            case 62:
                return " friends";
            case 63:
                return " friend";
            case 64:
                return "Invite ";
            case 65:
                return "Spend ";
            case 66:
                return " coins";
            case 67:
                return " character customization";
            case 68:
                return " character outfits";
            case 69:
                return " character outfit";
            case 70:
                return " customization colors";
            case 71:
                return " customization color";
            case 72:
                return "Throw ";
            case 73:
                return "Watch ";
            case 74:
                return "Drag ";
            case 75:
                return " visitors";
            case 76:
                return " bonus videos";
            case 77:
                return " visitors to the the lobby!";
            case 78:
                return " staff";
            case 79:
                return "Send ";
            case 80:
                return " gifts";
            case 81:
                return " gift";
            case 82:
                return "Welcome to <color=#808080><b>Pocket Hotel</b></color>, i will help you build a succesful hotel!";
            case 83:
                return "First, set your hotel name and username";
            case 84:
                return "Nice!, now lets build some rooms";
            case 85:
                return "To keep the shop open, click on the <color=#808080><b>lock button</b></color> in the <color=#808080><b>top right corner</b></color>";
            case 86:
                return "Good, now drag <color=#808080><b>3</b></color> rooms and add them to your hotel!";
            case 87:
                return "Perfect!, click here to finish building";
            case 88:
                return "Your hotel is closed right now!, Click here to start a shift";
            case 89:
                return "You can hire friends or purchase staff outfits to <color=#808080><b>reduce wages</b></color>";
            case 90:
                return "Congratulations! Your hotel is open for business and will keep runing even after you log off";
            case 91:
                return "Keep your hotel running so you keep receiving money!, Well, see you soon.";
            case 92:
                return "You leveled up, huh? Apparently you're serious about this";
            case 93:
                return "Lets get some decoration so you can improve your <color=#808080><b>stars</b></color>";
            case 94:
                return "Drag into the room, you can decorate <color=#808080><b>only</b></color> in the rooms that do not black out";
            case 95:
                return "More stars means more visitors, more visitors means <color=#808080><b>more money!</b></color>";
            case 96:
                return "Sometimes you have to <color=#808080><b>fix</b></color> room issues, <color=#808080><b>follow the tips</b></color> to learn how!";
            case 97:
                return "You are the man, you figure it out!, to help you fix, you can build staff rooms";
            case 98:
                return "Connect with facebook to see your friends!";
            case 99:
                return "You can visit friends to get daily bonuses or hire them to reduce wages!, and of course brag.";
            case 100:
                return "Your hotel is getting small, here you can increase your hotel's maximum size";
            case 101:
                return "Oops, a visitor would have liked to use a gym, but you didn't have one";
            case 102:
                return "You can find gyms here!";
            case 103:
                return "Incredible, you are ready to run your own hotel! I'll keep an eye on what you can do";
            case 104:
                return "Dont forget to support us on facebook to get <color=#808080><b>special rewards!</b></color> See ya!";
            case 105:
                return "To help you get started you will receive <color=#808080><b>30,000</b></color> coins!";
            case 106:
                return "Too short";
            case 107:
                return "Click on the money bag <color=#808080><b>in the lobby</b></color> to get some coins!";
            case 108:
                return "Click on the <color=#808080><b>left corner</b></color> to return home";
            case 109:
                return "Not enough coins";
            case 110:
                return "Not enough gems";
            case 111:
                return "Too short";
            case 112:
                return "Not enough level";
            case 113:
                return "You cant do that";
            case 114:
                return "Need more items";
            case 115:
                return "Need more blocks";
            case 116:
                return "Friends limit reached: 24";
            case 117:
                return "Friend requests limit reached: 10";
            case 118:
                return "Requests limit reached: 10";
            case 119:
                return "Already hired";
            case 120:
                return "Already purchased";
            case 121:
                return "Infinite";
            case 122:
                return "Not enough stars";
            case 123:
                return "Invalid";
            case 124:
                return "Already Redeemed";
            case 125:
                return "Room";
            case 126:
                return "Decoration";
            case 127:
                return "Outside";
            case 128:
                return "Staff";
            case 129:
                return "Special Item";
            case 130:
                return "Body Color";
            case 131:
                return "Character Outfit";
            case 132:
                return "Character Mouth";
            case 133:
                return "Character Extra";
            case 134:
                return "Extra Color";
            case 135:
                return "Character Eyes";
            case 136:
                return "Eyes Color";
            case 137:
                return "Glasses";
            case 138:
                return "Glasses Color";
            case 139:
                return "Glass Color";
            case 140:
                return "Character Hair";
            case 141:
                return "Hair Color";
            case 142:
                return " multipliplied by ";
            case 143:
                return " sent you a friend request";
            case 144:
                return " accepted your friend request";
            case 145:
                return " sent you a gift";
        }
    }
    string Spanish(int x)
    {
        switch (x)
        {
            default:
                return "";
            case 0:
                return " bloques usados";
            case 1:
                return "Haz desbloqueado: ";
            case 2:
                return "El siguiente nivel esta a ";
            case 3:
                return " puntos de distancia y desbloquearas: ";
            case 4:
                return " y ";
            case 5:
                return "Expansion de 24 bloques";
            case 6:
                return "Expansion de 28 bloques";
            case 7:
                return "Expansion de 34 bloques";
            case 8:
                return "Expansion de 42 bloques";
            case 9:
                return "Expansion de 52 bloques";
            case 10:
                return "Expansion de 64 bloques";
            case 11:
                return "Expansion de 76 bloques";
            case 12:
                return "Expansion de 92 bloques";
            case 13:
                return "Expansion de 116 bloques";
            case 14:
                return "Expansion de 128 bloques";
            case 15:
                return "Expansion de 144 bloques";
            case 16:
                return "Expansion de 164 bloques";
            case 17:
                return "Expansion de 176 bloques";
            case 18:
                return "Expansion de 194 bloques";
            case 19:
                return "Expansion de 210 bloques";
            case 20:
                return "Expansion de 224 bloques";
            case 21:
                return "Expansion de 256 bloques";
            case 22:
                return "Expansion de 272 bloques";
            case 23:
                return "Expansion de 312 bloques";
            case 24:
                return " puntos de distancia";
            case 25:
                return " Atuendo";
            case 26:
                return "hora";
            case 27:
                return "Arregla ";
            case 28:
                return "Limpia ";
            case 29:
                return " cuarto";
            case 30:
                return " cuartos";
            case 31:
                return "Responde ";
            case 32:
                return " llamadas de cuarto";
            case 33:
                return " tuberias";
            case 34:
                return "Encuentra ";
            case 35:
                return " llaves";
            case 36:
                return " energias de cuarto";
            case 37:
                return "Recoge ";
            case 38:
                return " propinas";
            case 39:
                return "Visita ";
            case 40:
                return " amigos";
            case 41:
                return " amigo";
            case 42:
                return "Compra ";
            case 43:
                return " decoraciones";
            case 44:
                return " decoracion";
            case 45:
                return " pinturas de cuarto";
            case 46:
                return " pintura de cuarto";
            case 47:
                return " pisos de cuarto";
            case 48:
                return " piso de cuarto";
            case 49:
                return " camas";
            case 50:
                return " cama";
            case 51:
                return " objetos de pared";
            case 52:
                return " objeto de pared";
            case 53:
                return " objetos de piso";
            case 54:
                return " objetos de piso";
            case 55:
                return " objetos de techo";
            case 56:
                return " objeto de techo";
            case 57:
                return "Estalla ";
            case 58:
                return " nubes";
            case 59:
                return " nubes de recompensa";
            case 60:
                return " atuendos de staff";
            case 61:
                return " atuendo de staff";
            case 62:
                return " amigos";
            case 63:
                return " amigo";
            case 64:
                return "Invita ";
            case 65:
                return "Gasta ";
            case 66:
                return " monedas";
            case 67:
                return " personalizacion de personaje";
            case 68:
                return " atuendos de personaje";
            case 69:
                return " atuendo de personaje";
            case 70:
                return "colores de personalizacion";
            case 71:
                return "color de personalizacion";
            case 72:
                return "Lanza ";
            case 73:
                return "Ve ";
            case 74:
                return "Arrastra ";
            case 75:
                return " visitantes";
            case 76:
                return " videos de bonus";
            case 77:
                return " visitantes al lobby!";
            case 78:
                return " staff";
            case 79:
                return "Envia ";
            case 80:
                return " regalos";
            case 81:
                return " regalo";
            case 82:
                return "Bienvenido a <color=#808080><b>Pocket Hotel</b></color>, te ayudare a construir el hotel de tus sueños!";
            case 83:
                return "Primero ingresa el nombre de tu hotel y tu nombre de usuario";
            case 84:
                return "Genial!, ahora, a construir algunos cuartos";
            case 85:
                return "Para mantener la tienda abierta, haz click en el <color=#808080><b>candado</b></color> en la <color=#808080><b>esquina superior derecha</b></color>";
            case 86:
                return "Bien, ahora arrastra <color=#808080><b>tres</b></color> cuartos y añadelos a tu hotel!";
            case 87:
                return "Perfecto!, haz click aqui para finalizar la construccion";
            case 88:
                return "Tu hotel esta cerrado actualmente!, haz click aqui para iniciar un turno";
            case 89:
                return "Puedes emplear a tus amigos o comprar atuendos para <color=#808080><b>reducir</b></color> salarios";
            case 90:
                return "Felicidades! Tu hotel esta listo para el publico y estara funcionando incluso cuando no estes";
            case 91:
                return "Manten tu hotel activo para seguir recibiendo dinero. Bien, nos vemos";
            case 92:
                return "Subiste nivel, ¿eh? Aparentemente te lo estas tomando en serio";
            case 93:
                return "Vamos a decorar un poco para mejorar tus <color=#808080><b>estrellas</b></color>";
            case 94:
                return "Arrastra al cuarto para decorar, <color=#808080><b>solo</b></color> se pueden decorar aquellos que no se obscurecen";
            case 95:
                return "Mas estrellas significan mas visitates y mas visitantes significa <color=#808080><b>mas dinero!</b></color>";
            case 96:
                return "Algunas veces tendras que reparar habitaciones, <color=#808080><b>sigue los tips</b></color> para aprender como!";
            case 97:
                return "Excelente, lo hiciste!, para ayudarte a reparar, puedes construir cuartos de mantenimiento";
            case 98:
                return "Conectate con facebook para ver a tus amigos!";
            case 99:
                return "Puedes visitar a tus amigos para obtener recompensas o emplearlos para reducir salarios!, o claro, presumir.";
            case 100:
                return "Tu hotel se esta quedando pequeño, aqui puedes expandir la maxima capacidad de tu hotel";
            case 101:
                return "Oops, un visitante hubiera querido usar el gimnasio, pero no tenias uno";
            case 102:
                return "Puedes encontrar gimnasios aqui";
            case 103:
                return "Increible, estas listo para dirigir tu propio hotel! Voy a seguirte de cerca para ver lo que puedes hacer";
            case 104:
                return "No olvides seguirnos en facebook para obtener <color=#808080><b>recompensas especiales!</b></color> nos vemos!";
            case 105:
                return "Para ayudarte a iniciar, aqui tienes <color=#808080><b>30,000</b></color> monedas!";
            case 106:
                return "Demasiado corto";
            case 107:
                return "Haz click en la <color=#808080><b>bolsa en el lobby</b></color> para obtener algunas monedas";
            case 108:
                return "Haz click en la <color=#808080><b>esquina izquierda</b></color> para regresar a casa";
            case 109:
                return "Monedas insuficientes";
            case 110:
                return "Gemas insuficientes";
            case 111:
                return "Muy corto";
            case 112:
                return "Nivel insuficiente";
            case 113:
                return "No puedes hacer eso";
            case 114:
                return "Necesitas mas objetos";
            case 115:
                return "Necesitas mas bloques";
            case 116:
                return "Limite de amigos: 24";
            case 117:
                return "Limite de solicitudes de amigo: 10";
            case 118:
                return "Limite de solicitudes: 10";
            case 119:
                return "Ya contratado";
            case 120:
                return "Ya adquirido";
            case 121:
                return "Infinito";
            case 122:
                return "Estrellas insuficientes";
            case 123:
                return "Invalido";
            case 124:
                return "Ya redimido";
            case 125:
                return "Cuarto";
            case 126:
                return "Decoracion";
            case 127:
                return "Exterior";
            case 128:
                return "Staff";
            case 129:
                return "Objeto especial";
            case 130:
                return "Color de Cuerpo";
            case 131:
                return "Atuendo de Personaje";
            case 132:
                return "Boca de Personaje";
            case 133:
                return "Extra de Personaje";
            case 134:
                return "Color de Extra";
            case 135:
                return "Ojos de Personaje";
            case 136:
                return "Color de Ojos";
            case 137:
                return "Lentes";
            case 138:
                return "Color de Lentes";
            case 139:
                return "Color de Lente";
            case 140:
                return "Cabello de Personaje";
            case 141:
                return "Color de Cabello";
            case 142:
                return " multiplicada por ";
            case 143:
                return " te envio una solicitud de amistad";
            case 144:
                return " acepto tu solicitud de amistad";
            case 145:
                return " te envio un regalo";

        }
    }
}
