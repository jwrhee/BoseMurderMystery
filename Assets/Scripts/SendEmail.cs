using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendEmail : MonoBehaviour
{
    public void OnLinkClicked()
    {
        string email = "ted@mildbeast.com";
        string subject = MyEscapeURL("Death Within Earshot Support");
        string body = MyEscapeURL("My question/issue: ");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}