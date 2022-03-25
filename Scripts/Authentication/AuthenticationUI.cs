using UnityEngine;
using UnityEngine.UI;


namespace Authentication
{
    public class AuthenticationUI : Authentication
    {
        void OnEnable()
        {
           // if(!string.IsNullOrEmpty(PlayerPrefs.GetString(PlayerPrefsData.ID)))
        //    Login(PlayerPrefs.GetString(PlayerPrefsData.EMAIL), PlayerPrefs.GetString(PlayerPrefsData.PASSWORD));

            Register();
           
        }

        [SerializeField] private InputField sigunUpEmail, signUpPassword, signUpfirstName, signUplastName;

        [SerializeField] private InputField loginEmail, loginPassword;

        [SerializeField] private InputField forgetPasswordEmail;

        [SerializeField] private InputField verifyOTPemail, verifyOTPotp;

        [SerializeField] private InputField resetPasswordemail, resetPasswordpassword, resetPasswordOTP;


        public void LoginUI()
        {
            UpdateUser(loginEmail.text, "fff");
        }

        public void LoginSocialUI()
        {
            // LoginBySocial(loginEmail.text, loginPassword.text);
        }

        public void SignUpUI()
        {
            SignUp(sigunUpEmail.text, signUpPassword.text, signUpfirstName.text, signUplastName.text);
        }

        public void ForgetPasswordUI()
        {
            ForgetPassword(forgetPasswordEmail.text);
        }

        public void VerifyOTPUI()
        {
            VerifyOTP(int.Parse(verifyOTPotp.text));
        }

        public void ResetPasswordUI()
        {
            ResetPassword(resetPasswordpassword.text);
        }
    }
}