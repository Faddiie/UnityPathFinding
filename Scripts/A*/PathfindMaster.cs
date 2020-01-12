using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GridMaster;
using System.Threading;

namespace Pathfinding
{
    //This class controls the threads
    public class PathfindMaster : MonoBehaviour
    {
        //Singleton
        private static PathfindMaster instance;
        void Awake()
        {
            instance = this;
        }
        public static PathfindMaster GetInstance()
        {
            return instance;
        }

        //The maximum simultaneous threads we allow to open
        public int MaxJobs = 3;

        //Delegates are a variable that points to a function
        public delegate void PathfindingJobComplete(List<Node> path);

        private List<Pathfinder> currentJobs;
        private List<Pathfinder> todoJobs;

        void Start()
        {
            currentJobs = new List<Pathfinder>();
            todoJobs = new List<Pathfinder>();
        }
   
        void Update() 
        {
            /*
             * Another way to keep track of the threads we have open would have been to create 
             * a new thread for it but we can also just use Unity's main thread too since this class
             * derives from monoBehaviour
             */

            int i = 0;

            while(i < currentJobs.Count)
            {
                if(currentJobs[i].jobDone)
                {
                    currentJobs[i].NotifyComplete();
                    currentJobs.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            if(todoJobs.Count > 0 && currentJobs.Count < MaxJobs)
            {
                Pathfinder job = todoJobs[0];
                todoJobs.RemoveAt(0);
                currentJobs.Add(job);

                //Start a new thread

                Thread jobThread = new Thread(job.FindPath);
                jobThread.Start();				
            }
        }

        public void RequestPathfind(Node start, Node target, PathfindingJobComplete completeCallback)
        {
            Pathfinder newJob = new Pathfinder(start, target, completeCallback);
            todoJobs.Add(newJob);
        }
    }
}
