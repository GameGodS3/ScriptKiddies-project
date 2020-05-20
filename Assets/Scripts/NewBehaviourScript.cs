    /*
    DATE:21/2/2020
    
    BRIEF DESCRIPTION:

    The below code is responsible for
    1)accelerating/braking
    2)changing gears
    3)displaying error log

    INPUT:NewBehaviourScript.cs
    OUTPUT:Prints violation in console if violation occurs and car moves
    */


    using UnityEngine;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using System.IO;

     
     
    public class NewBehaviourScript: MonoBehaviour {
     
        public float enginePower = 150.0f;
        public float power = 0.0f;
        public float brake = 0.0f;
        public float brake1=0.0f;
        public float steer = 0.0f;
        public float maxSteer = 70.0f;
        public float clutch = 0.0f;
        public float gear = 1.0f;
        public int gearno = 0;
        public WheelCollider Wheel_LF;
        public WheelCollider Wheel_RF;
        public WheelCollider Wheel_LR;
        public WheelCollider Wheel_RR;
        public Transform wheelFLTrans;
        public Transform wheelFRTrans;
        public Transform wheelRLTrans;
        public Transform wheelRRTrans;
        public float decellarationSpeed = 120;
        public float clutchviolation = 0.0f;
        public float collisionerror = 0.0f;
        public float Speed = 0.0f;
        private Rigidbody _rb;
       
 

               
     
        void Start()
        {
            GetComponent<Rigidbody>().centerOfMass=new Vector3(0f,-0.5f,0.3f);
           // _rb = GetComponent<Rigidbody>();

        }

        
           
    
     
        void Update ()
        {
            
            gear = 50.0f * gearno;
            power=Input.GetAxis("Vertical") * enginePower * Time.deltaTime * gear * 1.0f * 30000.0f;
            steer=Input.GetAxis("Horizontal") * maxSteer;
            brake=Input.GetKey("space") ? GetComponent<Rigidbody>().mass * 0.1f: 0.0f;
            brake1=Input.GetKey("backspace") ? GetComponent<Rigidbody>().mass * 1000.0f: 0.0f;
            
            
            //Rotates Wheels  
            wheelFLTrans.Rotate(Wheel_LF.rpm/60*360*Time.deltaTime,0,0); 
            wheelFRTrans.Rotate(Wheel_RF.rpm/60*360*Time.deltaTime,0,0);
            wheelRLTrans.Rotate(Wheel_LR.rpm/60*360*Time.deltaTime,0,0);
            wheelRRTrans.Rotate(Wheel_RR.rpm/60*360*Time.deltaTime,0,0);
            
            //Steering 
            Wheel_LF.steerAngle=steer;
            Wheel_RF.steerAngle=steer;

            clutcherror();

           // SpeedText.text = _rb.velocity.magnitude.ToString();
;
            
            if(brake1 > 0.0) //When brake is pressed
            {
                Wheel_LF.brakeTorque=brake1;
                Wheel_RF.brakeTorque=brake1;
                Wheel_LR.brakeTorque=brake1;
                Wheel_RR.brakeTorque=brake1;
                Wheel_LR.motorTorque=0.0f;
                Wheel_RR.motorTorque=0.0f;
            }
            
            else  //When brake not pressed
            {
                Wheel_LF.brakeTorque=0;
                Wheel_RF.brakeTorque=0;
                Wheel_LR.brakeTorque=0;
                Wheel_RR.brakeTorque=0;
                Wheel_LR.motorTorque=power;
                Wheel_RR.motorTorque=power;
            }
            
            //When there is no acceleration and clutch not pressed, vehicle has to slow down automatically due to engine friction     
            if ((Input.GetButton("Vertical")==false)&&(!(Input.GetKey("x"))||(gearno!=0)))  
            {
                Wheel_RR.brakeTorque = decellarationSpeed;
                Wheel_LR.brakeTorque = decellarationSpeed;
                
            }
            else
            {
                Wheel_RR.brakeTorque = 0;
                Wheel_LR.brakeTorque = 0;
            }
        }

       /* void _Speed()
        {
            Speed=SEDAN.velocity.magnitude*3.6;
            print(Speed);
        }*/

        void clutcherror()
        {
            if(Input.GetKey("x")) // input clutch 
            {
                 if (Input.GetKey("l")) // input gear 3
                 {  
                    gearno=90;
                }
                 else if(Input.GetKey("o")) // input gear 2
                  { 
                    gearno=400;
                }
                 else if (Input.GetKey("k")) // input gear 1
                 {  
                    gearno = 1100;
                }
                else if (Input.GetKey("n")) // input Neutral
                 {  
                    gearno = 0;
                }
            }

            //Clutch Violation
            else if((Input.GetKey("l"))||(Input.GetKey("o"))||(Input.GetKey("k"))||(Input.GetKey("n"))) 
            {
                print("Violation");
                clutchviolation = 1;
            }
        }

        //To log collisions
        void OnCollisionEnter(Collision collision) 
        {
            string path=@"D:\\hackathon\\SHIT\\Assets\\Log\\Violation.log";
            // DirectoryInfo[] cDirs = new DirectoryInfo(@"D:\\hackathon\\SHIT\\Assets\\Log").GetDirectories();
            
            if (collision.gameObject.name!=null)
            {    
                print("Collision");
                collisionerror++;
                using(StreamWriter sw = File.AppendText(path))
                {
                sw.WriteLine("Violation");  
                }
                //public static System.IO.StreamWriter
                //AppendText(string "D:\\hackathon\\SHIT\\Assets\\Log\\Violation.log");

                //using(StreamWriter sw = new StreamWriter("Violation.log"))
                //{ sw.AppendText("Violation");  
               // }
                //StreamWriter sw = new StreamWriter(@ "D:\\hackathon\\SHIT\\Assets\\Log\\Violation.log", false);  
                
               // sw.close();   
            }
        }

       
    }


