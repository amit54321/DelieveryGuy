using System.Collections.Generic;
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
        public List<Sprite> profilePictures;
        public Image avatarImage;
        int currentAvatar = 0;
        public void NextImage()
        {
            if (currentAvatar == profilePictures.Count-1)
            {
                currentAvatar = 0;
            }
            currentAvatar++;
            avatarImage.sprite = profilePictures[currentAvatar];
        }
        public void PreviousImage()
        {
            if (currentAvatar == 0)
            {
                currentAvatar = profilePictures.Count - 1;
            }
            currentAvatar--;
            avatarImage.sprite = profilePictures[currentAvatar];
        }
        public void LoginUI()
        {
            if (!string.IsNullOrEmpty(loginEmail.text))
            {
                ButtonSOund.instance.Play();
                UpdateName(loginEmail.text, currentAvatar);
            }
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