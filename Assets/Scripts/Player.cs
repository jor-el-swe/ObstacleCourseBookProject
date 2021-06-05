using UnityEngine;

public class Player : MonoBehaviour
{
    
    //References
    [Header("References")] 
    public Transform trans;
    public Transform modelTrans;
    public CharacterController characterController;
    
    //Movement
    [Header("Movement")] 
    [Tooltip("Units moved per second at maximum speed.")]
    public float moveSpeed = 24;

    [Tooltip("Time, in seconds, to reach maximum speed.")]
    public float timeToMaxSpeed = 0.26f;
    private float VelocityGainPerSecond => moveSpeed / timeToMaxSpeed;

    [Tooltip("Time, in seconds, to go from maximum speed to stationary.")]
    public float timeToLoseMaxSpeed = .2f;
    private float VelocityLossPerSecond => moveSpeed / timeToLoseMaxSpeed;

    [Tooltip("Multiplier for momentum when attempting to move in a direction opposite the current traveling direction" +
             "(e.g. trying to move right when already moving left). ")]
    public float reverseMomentumMultiplier = 2.2f;
    
    private Vector3 _movementVelocity = Vector3.zero;
    
    
    //Death and Respawning
    [Header("Death and Respawning")]
    [Tooltip("How long after the player's death, in seconds, before they are respawned?")]
    public float respawnWaitTime = 2f;

    private bool dead = false;
    
    private Vector3 spawnPoint;
    private Quaternion spawnRotation;

    public void Respawn()
    {
        dead = false;
        trans.position = spawnPoint;
        modelTrans.rotation = spawnRotation;
        enabled = true;
        characterController.enabled = true;
        modelTrans.gameObject.SetActive(true);
    }
    
    public void Die()
    {
        if (!dead)
        {
            dead = true;
            Invoke("Respawn", respawnWaitTime);
            _movementVelocity = Vector3.zero;
            enabled = false;
            characterController.enabled = false;
            modelTrans.gameObject.SetActive(false);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = trans.position;
        spawnRotation = trans.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();    
        
        //test code for "Die"
        if (Input.GetKeyUp(KeyCode.K))
        {
            Die();
        }
    }

    void Movement()
    {
         //FORWARD AND BACKWARD MOVEMENT

        //If W or the up arrow key is held:
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (_movementVelocity.z >= 0) //If we're already moving forward
                //Increase Z velocity by VelocityGainPerSecond, but don't go higher than 'movespeed':
                _movementVelocity.z = Mathf.Min(moveSpeed,_movementVelocity.z + VelocityGainPerSecond * Time.deltaTime);

            else //Else if we're moving back
                //Increase Z velocity by VelocityGainPerSecond, using the reverseMomentumMultiplier, but don't raise higher than 0:
                _movementVelocity.z = Mathf.Min(0,_movementVelocity.z + VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
        }
        //If S or the down arrow key is held:
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (_movementVelocity.z > 0) //If we're already moving forward
                _movementVelocity.z = Mathf.Max(0,_movementVelocity.z - VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);

            else //If we're moving back or not moving at all
                _movementVelocity.z = Mathf.Max(-moveSpeed,_movementVelocity.z - VelocityGainPerSecond * Time.deltaTime);
        }
        else //If neither forward nor back are being held
        {
            //We must bring the Z velocity back to 0 over time.
            if (_movementVelocity.z > 0) //If we're moving up,
                //Decrease Z velocity by VelocityLossPerSecond, but don't go any lower than 0:
                _movementVelocity.z = Mathf.Max(0,_movementVelocity.z - VelocityLossPerSecond * Time.deltaTime);
            
            else //If we're moving down,
                //Increase Z velocity (back towards 0) by VelocityLossPerSecond, but don't go any higher than 0:
                _movementVelocity.z = Mathf.Min(0,_movementVelocity.z + VelocityLossPerSecond * Time.deltaTime);
        }

        //RIGHT AND LEFT MOVEMENT:

        //If D or the right arrow key is held:
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (_movementVelocity.x >= 0) //If we're already moving right
                //Increase X velocity by VelocityGainPerSecond, but don't go higher than 'movespeed':
                _movementVelocity.x = Mathf.Min(moveSpeed,_movementVelocity.x + VelocityGainPerSecond * Time.deltaTime);

            else //If we're moving left
                //Increase x velocity by VelocityGainPerSecond, using the reverseMomentumMultiplier, but don't raise higher than 0:
                _movementVelocity.x = Mathf.Min(0,_movementVelocity.x + VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (_movementVelocity.x > 0) //If we're already moving right
                _movementVelocity.x = Mathf.Max(0,_movementVelocity.x - VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);

            else //If we're moving left or not moving at all
                _movementVelocity.x = Mathf.Max(-moveSpeed,_movementVelocity.x - VelocityGainPerSecond * Time.deltaTime);
        }

        else //If neither right nor left are being held
        {
            //We must bring the X velocity back to 0 over time.
            
            if (_movementVelocity.x > 0) //If we're moving right,
                //Decrease X velocity by VelocityLossPerSecond, but don't go any lower than 0:
                _movementVelocity.x = Mathf.Max(0,_movementVelocity.x - VelocityLossPerSecond * Time.deltaTime);
            
            else //If we're moving left,
                //Increase X velocity (back towards 0) by VelocityLossPerSecond, but don't go any higher than 0:
                _movementVelocity.x = Mathf.Min(0,_movementVelocity.x + VelocityLossPerSecond * Time.deltaTime);
        }

        //If the player is moving in either direction (left/right or up/down):
        if (_movementVelocity.x != 0 || _movementVelocity.z != 0)
        {
            //Applying the movement velocity:
            characterController.Move(_movementVelocity * Time.deltaTime);
            
            //Keeping the model holder rotated towards the last movement direction:
            modelTrans.rotation = Quaternion.Slerp(modelTrans.rotation,Quaternion.LookRotation(_movementVelocity),.18F);
        }
    }
}
