using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.AI;


public class DeadPlayerController : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Talking,
        Patrol
    }

    public enum DeadPlayers
    {
        TutorialFireman,
        MagicalYo,
        FallingFurniture1,
        FallingFurniture2,
        LeverRoom,
        BossHallway
    }

    public DeadPlayers playerType;
    public FSMStates currentState;
    public int maxSpeakingDistance = 5;
    public int deadPlayerSpeed = 3;
    public int maxIdleDistance = 15;
    public AudioClip[] orderedCommandClips;
    public AudioClip[] orderedResponseClips;
    public Dictionary<string, AudioClip> commands;
    public Dictionary<string, AudioClip> griffenCommandResponsesClips;
    public Dictionary<string, string> griffenCommandResponses;

    NavMeshAgent agent;

    Animator anim;
    public GameObject griffen;
    string nextCommand;
    bool dialogueCompleted;
    Vector3 nextDestination;
    int currDestinationIndex = 0;
    float distanceToPlayer;
    bool isCommandPlaying;
    bool isResponsePlaying;
    bool isWaitingForResponse;
    float commandTimeRemaining;
    float responseTimeRemaining;
    bool hasCurrDeadPlayerCommandBeenSpoken;
    float originalFov;
    bool isStart;

    public GameObject commandUIDialogueBox;
    public GameObject responseUIDialogueBox;

    public GameObject[] wanderPoints;
    public Text commandUIText;
    public Text responseUIText;

    AudioClip lastAudio;
    AudioSource audioSource;

    float originalVolume;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        print("STARTED");
        isStart = true;
        dialogueCompleted = false;
        anim = GetComponent<Animator>();
        currentState = FSMStates.Patrol;
        nextDestination = wanderPoints[currDestinationIndex].transform.position;
        currDestinationIndex = (currDestinationIndex + 1) % wanderPoints.Length;
        agent.SetDestination(nextDestination);
        hasCurrDeadPlayerCommandBeenSpoken = false;
        griffen = GameObject.FindGameObjectWithTag("Player");
       // commandUIDialogueBox = GameObject.FindGameObjectWithTag("CommandComponent");
       // responseUIDialogueBox = GameObject.FindGameObjectWithTag("ResponseComponent");
        //commandUIText = GameObject.FindGameObjectWithTag("Command").GetComponent<Text>();
        //responseUIText = GameObject.FindGameObjectWithTag("Response").GetComponent<Text>();
        print("commanduitext " + commandUIDialogueBox);
        commandUIDialogueBox.SetActive(false);
        responseUIDialogueBox.SetActive(false);

        GameObject mainCamera = GameObject.FindGameObjectWithTag("CameraController");
        CinemachineVirtualCamera camera = mainCamera.GetComponent<CinemachineVirtualCamera>();
        float fov = camera.m_Lens.FieldOfView;
        originalFov = camera.m_Lens.FieldOfView;
        distanceToPlayer = Vector3.Distance(transform.position, griffen.transform.position);
        audioSource = GetComponent<AudioSource>();
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        originalVolume = cam.GetComponent<AudioSource>().volume;
        audioSource.volume = audioSource.volume * 30;
        SetCommandsAndResponsesPerDeadPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(gameObject.transform.position, griffen.transform.position);
        // print("commandTimeRemaining=" + commandTimeRemaining + ", responseTimeRemaining=" + responseTimeRemaining);
        if (commandTimeRemaining <= 0 && isCommandPlaying)
        {
            isCommandPlaying = false;
            //isGrifSpeaking = false;
        }
        else if (isCommandPlaying)
        {
            commandTimeRemaining -= Time.deltaTime;
        }

        if (responseTimeRemaining <= 0 && isResponsePlaying)
        {
            isResponsePlaying = false;
            //isGrifSpeaking = false;
        }
        else if (isResponsePlaying)
        {
            responseTimeRemaining -= Time.deltaTime;
        }

        switch (currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Idle:
                UpdateIdleState();
                break;
            case FSMStates.Talking:
                UpdateTalkingState();
                break;
        }
    }

    void UpdateTalkingState()
    {
        anim.SetBool("Talking", true);

        commandUIDialogueBox.SetActive(true);
        responseUIDialogueBox.SetActive(true);
        agent.SetDestination(transform.position);

        commandUIText.text = nextCommand;
        responseUIText.text = griffenCommandResponses[nextCommand];

        GameObject mainCamera = GameObject.FindGameObjectWithTag("CameraController");
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam.GetComponent<AudioSource>().volume = 0.1f;

        CinemachineVirtualCamera camera = mainCamera.GetComponent<CinemachineVirtualCamera>();
        float fov = camera.m_Lens.FieldOfView;
        camera.m_Lens.FieldOfView = Mathf.Lerp(fov, 30, Time.deltaTime * 2);

        bool shouldAutoCommand = (!isResponsePlaying && !isWaitingForResponse);

        if (Input.GetKeyDown(KeyCode.Tab) || shouldAutoCommand)
        {
            if (isWaitingForResponse)
            {
                //   if (isCommandPlaying)
                //  {
                //Keep waiting
                //  }
                //  else
                //  {
                // Play the response
                PlayNewClip(griffenCommandResponsesClips[nextCommand]);
                //AudioSource.PlayClipAtPoint(griffenCommandResponsesClips[nextCommand], griffen.transform.position, 1);
                responseTimeRemaining = 3;
                isResponsePlaying = true;
                isWaitingForResponse = false;
                //if (!isStart)
                //{
                UpdateCommands(true);
                //}
                // isStart = false;
                //}
            }
            else
            {
                //  if (isResponsePlaying)
                // {
                // Keep Waiting
                //   } else
                // {
                PlayNewClip(commands[nextCommand]);
                //lastAudio.stop();
                // AudioSource.PlayClipAtPoint(commands[nextCommand], gameObject.transform.position, 1);
                //lastAudio = commands[nextCommand];
                isCommandPlaying = true;
                commandTimeRemaining = 10;
                isWaitingForResponse = true;
                UpdateCommands(false);
                // }
            }
        }
        else if (distanceToPlayer > 10)
        {
            commandUIDialogueBox.SetActive(false);
            responseUIDialogueBox.SetActive(false);
            audioSource.Stop();

            if (distanceToPlayer <= maxIdleDistance)
            {
                anim.SetBool("Talking", false);
                cam.GetComponent<AudioSource>().volume = originalVolume;
                currentState = FSMStates.Idle;
            }
            else
            {
                cam.GetComponent<AudioSource>().volume = originalVolume;

                anim.SetBool("Talking", false);
                currentState = FSMStates.Patrol;
            }
            // Set back field of view
            camera.m_Lens.FieldOfView = originalFov;
        }
    }

    void UpdateCommands(bool isResponse)
    {
        if (!dialogueCompleted && isResponse)
        {
            if (commands.Count <= 1)
            {
                dialogueCompleted = true;
                // Next command repeated 
            }
            else
            {
                commands.Remove(nextCommand);
                griffenCommandResponsesClips.Remove(nextCommand);
                griffenCommandResponses.Remove(nextCommand);
                var e = commands.GetEnumerator();
                e.MoveNext();
                nextCommand = e.Current.Key;
                //AudioSource.PlayClipAtPoint(commands[nextCommand], transform.position);

            }
        }
    }

    void UpdateIdleState()
    {
        // print("IDLE!!!! Distance to place = " + distanceToPlayer);
        anim.SetBool("Relax", true);
        agent.SetDestination(transform.position);
        if (distanceToPlayer <= maxSpeakingDistance && Input.GetKeyDown(KeyCode.Tab))
        {
            currentState = FSMStates.Talking;
            anim.SetBool("Relax", false);
            isCommandPlaying = true;
            //lastAudio.stop();
            PlayNewClip(commands[nextCommand]);
            // AudioSource.PlayClipAtPoint(commands[nextCommand], gameObject.transform.position, 1);
            //lastAudio = commands[nextCommand];
            isWaitingForResponse = true;
            isResponsePlaying = false;
            commandTimeRemaining = commands[nextCommand].length;
        }
        else if (distanceToPlayer > maxIdleDistance)
        {
            currentState = FSMStates.Patrol;
            anim.SetBool("Relax", false);
        }

        FaceTarget(griffen.transform.position);
    }

    void UpdatePatrolState()
    {
        print("Patrolling!");
        anim.SetBool("Walk", true);
        agent.speed = 0.3f;
        agent.stoppingDistance = 0;

        if (Vector3.Distance(transform.position, nextDestination) < 1)
        {
            FindNextDestination();
            agent.SetDestination(nextDestination);
        }
        else if (distanceToPlayer <= maxIdleDistance)
        {
            anim.SetBool("Walk", false);
            currentState = FSMStates.Idle;
        }

        FaceTarget(nextDestination);
        transform.position = Vector3.MoveTowards
            (transform.position, nextDestination, deadPlayerSpeed * Time.deltaTime);
    }

    void SetCommandsAndResponsesPerDeadPlayer()
    {
        commands = new Dictionary<string, AudioClip>();
        griffenCommandResponsesClips = new Dictionary<string, AudioClip>();

        griffenCommandResponses = new Dictionary<string, string>();
        switch (playerType)
        {
            case DeadPlayers.TutorialFireman:
                // Command 1
                string commandFireman1 = "Oh great, another ambitious little explorer who thinks they can conquer the late Sir Vanderbilts’s mansion. Are you out of your mind? Everyone who tries, dies. Take it from me... I suggest you turn back while you still can.";
                string responseFireman1 = "No.";
                commands.Add(commandFireman1, orderedCommandClips[0]);
                griffenCommandResponses.Add(commandFireman1, responseFireman1);
                griffenCommandResponsesClips.Add(commandFireman1, orderedResponseClips[0]);
                // Command 2
                string commandFireman2 = "And what makes you think that YOU can make it past the evil mystical forces inside? Each fool that ventures through here looking for Sir Vanderbilt’s treasure perishes in these deathly mansion halls. You will be no different.";
                string responseFireman2 = "But you see.. I have roller skates!";
                commands.Add(commandFireman2, orderedCommandClips[1]);
                griffenCommandResponses.Add(commandFireman2, responseFireman2);
                griffenCommandResponsesClips.Add(commandFireman2, orderedResponseClips[1]);

                // Command 2
                string commandFireman3 = "You better not come find me in the afterlife, you have been warned.";
                string responseFireman3 = "Okay... crazy ghosty man.";
                commands.Add(commandFireman3, orderedCommandClips[2]);
                griffenCommandResponses.Add(commandFireman3, responseFireman3);
                griffenCommandResponsesClips.Add(commandFireman3, orderedResponseClips[2]);

                break;
            case DeadPlayers.MagicalYo:
                // Command 1
                string commandYoyo1 = "So… You’ve made it in…. Yayyyy….";
                string responseYoyo1 = "Yes I sure have! Which way to the treasure?";
                commands.Add(commandYoyo1, orderedCommandClips[0]);
                griffenCommandResponses.Add(commandYoyo1, responseYoyo1);
                griffenCommandResponsesClips.Add(commandYoyo1, orderedResponseClips[0]);

                // Command 2
                string commandYoyo2 = "HA! Not so fast kid, do you even have your YoYo on you?";
                string responseYoyo2 = "Excuse me? Yoyo? Like the toy?";
                commands.Add(commandYoyo2, orderedCommandClips[1]);
                griffenCommandResponses.Add(commandYoyo2, responseYoyo2);
                griffenCommandResponsesClips.Add(commandYoyo2, orderedResponseClips[1]);

                // Command 2
                string commandYoyo3 = "You can hate on it now, just have fun getting stuck in Sir Vanderbilt’s boobie traps in the next room… I thought I might have seen a lost Yoyo lying around here somewhere.";
                string responseYoyo3 = "Okay, whatever you say.";
                commands.Add(commandYoyo3, orderedCommandClips[2]);
                griffenCommandResponses.Add(commandYoyo3, responseYoyo3);
                griffenCommandResponsesClips.Add(commandYoyo3, orderedResponseClips[2]);
                break;
            case DeadPlayers.FallingFurniture1:
                // Command 1
                string commandFurn1 = "Holy shucks, a new friend! And this one’s still alive!";
                string responseFurn1 = "Excuse me...";
                commands.Add(commandFurn1, orderedCommandClips[0]);
                griffenCommandResponses.Add(commandFurn1, responseFurn1);
                griffenCommandResponsesClips.Add(commandFurn1, orderedResponseClips[0]);

                // Command 2
                string commandFurn2 = "There’s no point in delaying the inevitable, why don’t you just sit for a little chat with us? Stay for a while?";
                string responseFurn2 = "I need to go….";
                commands.Add(commandFurn2, orderedCommandClips[1]);
                griffenCommandResponses.Add(commandFurn2, responseFurn2);
                griffenCommandResponsesClips.Add(commandFurn2, orderedResponseClips[1]);

                // Command 2
                string commandFurn3 = "How does eternity sound?";
                string responseFurn3 = "I am going to go now...";
                commands.Add(commandFurn3, orderedCommandClips[2]);
                griffenCommandResponses.Add(commandFurn3, responseFurn3);
                griffenCommandResponsesClips.Add(commandFurn3, orderedResponseClips[2]);
                break;
            case DeadPlayers.FallingFurniture2:
                // Command 1
                string commandFurn21 = "I’m sorry about her, don’t be discouraged, you’re almost there, I can feel it!";
                string responseFurn21 = "How much longer do I have exactly?";
                commands.Add(commandFurn21, orderedCommandClips[0]);
                griffenCommandResponses.Add(commandFurn21, responseFurn21);
                griffenCommandResponsesClips.Add(commandFurn21, orderedResponseClips[0]);

                // Command 2
                string commandFurn22 = "I couldn’t tell you, I’ve been stuck in this room for the past 50 years.";
                string responseFurn22 = "Oh my god.";
                commands.Add(commandFurn22, orderedCommandClips[1]);
                griffenCommandResponses.Add(commandFurn22, responseFurn22);
                griffenCommandResponsesClips.Add(commandFurn22, orderedResponseClips[1]);

                // Command 2
                string commandFurn23 = "All I can say is, watch out for the furniture. If you can get all those pesky keys without being flattened by a bookcase, you will be on your way out of here. I’ve never been so skillful...";
                string responseFurn23 = "Cool! It sounds easy enough!";
                commands.Add(commandFurn23, orderedCommandClips[2]);
                griffenCommandResponses.Add(commandFurn23, responseFurn23);
                griffenCommandResponsesClips.Add(commandFurn23, orderedResponseClips[2]);
                break;
            case DeadPlayers.LeverRoom:
                // Command 1
                string commandLever1 = "I’m stuck here, I think I lost my yoyo in the other room!";
                string responseLever1 = "Oh that’s a shame. I hope someone finds it...";
                commands.Add(commandLever1, orderedCommandClips[0]);
                griffenCommandResponses.Add(commandLever1, responseLever1);
                griffenCommandResponsesClips.Add(commandLever1, orderedResponseClips[0]);

                // Command 2
                string commandLever2 = "Hey... what's that you're holding? That looks like --";
                string responseLever2 = "Nothing!... Bye!... Have a nice day!...";
                commands.Add(commandLever2, orderedCommandClips[1]);
                griffenCommandResponses.Add(commandLever2, responseLever2);
                griffenCommandResponsesClips.Add(commandLever2, orderedResponseClips[1]);

                break;
            case DeadPlayers.BossHallway:
                // Command 1
                string commandBoss1 = "Holy crap! I haven’t seen fresh meat make it this far to the treasure in decades! You're a boss!";
                string responseBoss1 = "Thank you, this is true! Now let me get my hands on all that gold!";
                commands.Add(commandBoss1, orderedCommandClips[0]);
                griffenCommandResponses.Add(commandBoss1, responseBoss1);
                griffenCommandResponsesClips.Add(commandBoss1, orderedResponseClips[0]);

                // Command 2
                string commandBoss2 = "Oh... So no one told you? Sir Vanderbilt guards his treasure with all of his life. You won’t be able to pry it from his dead hands.";
                string responseBoss2 = "I thought Sir Vanderbilt died hundreds of years ago?";
                commands.Add(commandBoss2, orderedCommandClips[1]);
                griffenCommandResponses.Add(commandBoss2, responseBoss2);
                griffenCommandResponsesClips.Add(commandBoss2, orderedResponseClips[1]);

                // Command 2
                string commandBoss3 = "He did.";
                string responseBoss3 = "Oh?...";
                commands.Add(commandBoss3, orderedCommandClips[2]);
                griffenCommandResponses.Add(commandBoss3, responseBoss3);
                griffenCommandResponsesClips.Add(commandBoss3, orderedResponseClips[2]);
                break;
        }
        var e = commands.GetEnumerator();
        e.MoveNext();
        nextCommand = e.Current.Key;
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
    }


    void FindNextDestination()
    {
        nextDestination = wanderPoints[currDestinationIndex].transform.position;
        currDestinationIndex = (currDestinationIndex + 1) % wanderPoints.Length;
    }

    void PlayNewClip(AudioClip newAudio)
    {
        audioSource.Stop();
        audioSource.clip = newAudio;
        audioSource.Play();
        lastAudio = newAudio;
    }
}
