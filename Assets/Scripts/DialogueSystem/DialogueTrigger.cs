namespace GameCreator.Characters
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using GameCreator.Core;
    using GameCreator.Core.Hooks;

    public class DialogueTrigger : MonoBehaviour
    {

        public Dialogue dialogue;
        public CharacterController charController;
        public PlayerCharacter playerChar;
        public Player player;

        public bool isHumanoid;


        public void Start()
        {
            playerChar = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerCharacter>();
            charController = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<CharacterController>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        public void Update()
        {
            if (dialogue.speaking && isHumanoid)
            {
                //playerChar.characterLocomotion.faceDirection = CharacterLocomotion.FACE_DIRECTION.CameraDirection; 

                //playerChar.characterLocomotion.isControllable = false;
                //charController.enabled = false;
                //player.enabled = false;

                Quaternion lookOnLook =
                Quaternion.LookRotation(player.transform.position - transform.position);

                transform.rotation =
                Quaternion.Slerp(transform.rotation, lookOnLook, 0.15f);

                //player.transform.LookAt(this.gameObject.transform);
            }

            if(!dialogue.speaking && !player.enabled)
            {
                //playerChar.characterLocomotion.faceDirection = CharacterLocomotion.FACE_DIRECTION.MovementDirection;

                //playerChar.characterLocomotion.isControllable = true;
                //charController.enabled = true;
                //player.enabled = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if(other.tag == "Player")
            {
                FindObjectOfType<DialogueManager>().EndDialogue(dialogue);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player" && Input.GetButtonUp("X") && !FindObjectOfType<DialogueManager>().textIsTyping)
            {
                if (dialogue.speaking && dialogue.inputBuffer % 2 == 0)
                {
                    FindObjectOfType<DialogueManager>().DisplayNextSentence(dialogue);
                    dialogue.inputBuffer++;
                }
                else if (!dialogue.speaking && dialogue.inputBuffer % 2 == 0)
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                    dialogue.speaking = true;
                    dialogue.inputBuffer++;
                }
                else
                {
                    dialogue.inputBuffer++;
                }
            }
        }

        public IEnumerator PlayHealEffect()
        {
            yield return new WaitForSeconds(1f);
        }
    }

}
