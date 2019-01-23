using System;
using UnityEngine;
using System.Collections;

public static class ConnectedUser
{
    public const string FAIL_TO_LOGIN = "Login fail";
    public const string FAIL_TO_CONNECT = "Not Connected";
    public const string LOGIN_SUCCESFUL = "Login Succesful";

    public static string REF_PATH_FOR_USER_DIRRECTORY = "/JSON/users/";
    public static User connectedUser { get; set; }

}
