using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoteBuster
{
    class Vote
    {
        Texture2D voteFace;
        string candidate;
        string mood;
        int X;
        int Y;
        Vector2 votePosition;
        bool isshot;
        bool isinscreen;
        bool outvote;
        bool isfree;
        bool isDecided;
        int yDir;

        public Vote(Texture2D voteFace, string candidate, string mood, int X, int Y, Vector2 votePosition, bool isshot, bool isinscreen, bool outvote, bool isfree, bool isDecided, int yDir)
        {
            this.voteFace = voteFace;
            this.candidate = candidate;
            this.mood = mood;
            this.X = X;
            this.Y = Y;
            this.votePosition = votePosition;
            this.isshot = isshot;
            this.isinscreen = isinscreen;
            this.outvote = outvote;
            this.isfree = isfree;
            this.isDecided = isDecided;
            this.yDir = yDir;
        }

        public Texture2D getVoteFace()
        {
            return voteFace;
        }

        public void setVoteFace(Texture2D newFace)
        {
            voteFace = newFace;
        }

        public string getCandidate()
        {
            return candidate;
        }

        public void setCandidate(string newCandidate)
        {
            candidate = newCandidate;
        }

        public string getMood()
        {
            return mood;
        }

        public void setMood(string newMood)
        {
            mood = newMood;
        }

        public int getX()
        {
            return X;
        }

        public void setX(int newX)
        {
            X = newX;
        }

        public int getY()
        {
            return Y;
        }

        public void setY(int newY)
        {
            Y = newY;
        }

        public Vector2 getvoteVector()
        {
            return votePosition;
        }

        public void setvoteVector(Vector2 newVoteVector)
        {
            votePosition = newVoteVector;
        }

        public bool isShooted()
        {
            return isshot;
        }

        public void setShot(bool s)
        {
            isshot = s;
        }

        public bool isShot(int x, int y)
        {
            if ((x > X && x < (X + voteFace.Width)) && (y > Y && y < (Y + voteFace.Height))) return true;
            return false;
        }

        public bool isInScreen()
        {
            return isinscreen;
        }

        public void setScreenStatus(bool news)
        {
            isinscreen = news;
        }

        public bool isOut()
        {
            return outvote;
        }

        public void setOutVote(bool newov)
        {
            outvote = newov;
        }

        public bool isFree()
        {
            return isfree;
        }

        public void setFree(bool newfree)
        {
            isfree = newfree;
        }

        public bool isVoteDecided()
        {
            return isDecided;
        }

        public void setVoteDecided(bool newdecision)
        {
            isDecided = newdecision;
        }

        public int getYDirection()
        {
            return yDir;
        }

        public void setYDirection(int newydir)
        {
            yDir = newydir;
        }

    }
}
