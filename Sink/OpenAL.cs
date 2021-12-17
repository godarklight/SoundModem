using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace SoundModem
{
    public class OpenALSink : ISink
    {
        const int BUFFER_SIZE = 48000;
        //AL buffers
        byte[] audioBuffer1;
        byte[] audioBuffer2;
        IntPtr audioBuffer1Ptr;
        IntPtr audioBuffer2Ptr;
        int freeBufferID = 0;
        int writePos = 0;
        //OpenAL state
        ALDevice device;
        ALContext context;
        int[] buffers = new int[2];
        int source = 0;

        public unsafe OpenALSink()
        {
            device = ALC.OpenDevice(null);
            context = ALC.CreateContext(device, (int*)null);
            ALC.MakeContextCurrent(context);

            //Get buffer handles
            AL.GenBuffers(2, buffers);
            AL.GenSources(1, ref source);

            //2 buffers so we can fill and swap them
            audioBuffer1 = new byte[BUFFER_SIZE];
            GCHandle handle1 = GCHandle.Alloc(audioBuffer1, GCHandleType.Pinned);
            audioBuffer1Ptr = handle1.AddrOfPinnedObject();

            //1 second buffer
            audioBuffer2 = new byte[BUFFER_SIZE];
            GCHandle handle2 = GCHandle.Alloc(audioBuffer2, GCHandleType.Pinned);
            audioBuffer2Ptr = handle2.AddrOfPinnedObject();
        }

        public void Write(byte[] sinkData, int sinkLength)
        {
            int bytesLeft = sinkLength;
            while (bytesLeft > 0)
            {
                //Write to free buffer
                byte[] freeBuffer = audioBuffer1;
                IntPtr freePtr = audioBuffer1Ptr;
                if (freeBufferID == 1)
                {
                    freeBuffer = audioBuffer2;
                    freePtr = audioBuffer2Ptr;
                }

                //Find write length
                int bufferWrite = BUFFER_SIZE - writePos;
                if (bufferWrite > bytesLeft)
                {
                    bufferWrite = bytesLeft;
                }

                //Copy data into buffer
                Array.Copy(sinkData, sinkLength - bytesLeft, freeBuffer, writePos, bufferWrite);
                bytesLeft -= bufferWrite;
                writePos += bufferWrite;

                if (writePos == BUFFER_SIZE)
                {
                    AL.BufferData(buffers[freeBufferID], ALFormat.Mono16, freePtr, BUFFER_SIZE, 48000);
                    AL.SourceQueueBuffer(source, buffers[freeBufferID]);
                    freeBufferID = (freeBufferID + 1) % 2;
                    writePos = 0;
                    AL.GetSource(source, ALGetSourcei.SourceState, out int sourceState);
                    if ((ALSourceState)sourceState != ALSourceState.Playing)
                    {
                        AL.SourcePlay(source);
                    }
                    AL.GetSource(source, ALGetSourcei.BuffersQueued, out int buffersQueued);
                    if (buffersQueued == 2)
                    {
                        bool wait = true;
                        while (wait)
                        {
                            AL.GetSource(source, ALGetSourcei.BuffersProcessed, out int buffersProcessed);
                            if (buffersProcessed > 0)
                            {
                                wait = false;
                            }
                            else
                            {
                                Thread.Sleep(10);
                            }
                        }
                        AL.GetSource(source, ALGetSourcei.BuffersProcessed, out int buffersProcessed2);
                        AL.SourceUnqueueBuffers(source, buffersProcessed2);
                    }
                }
            }
        }

        public void FillWithSilence()
        {
            //Write to free buffer
            byte[] freeBuffer = audioBuffer1;
            IntPtr freePtr = audioBuffer1Ptr;
            if (freeBufferID == 1)
            {
                freeBuffer = audioBuffer2;
                freePtr = audioBuffer2Ptr;
            }

            //Find write length
            int bufferWrite = BUFFER_SIZE - writePos;
            byte[] fillData = BitConverter.GetBytes((short)32767);
            while (writePos < BUFFER_SIZE)
            {
                freeBuffer[writePos] = fillData[0];
                freeBuffer[writePos + 1] = fillData[1];
                writePos += 2;
            }

            AL.BufferData(buffers[freeBufferID], ALFormat.Mono16, freePtr, BUFFER_SIZE, 48000);
            AL.SourceQueueBuffer(source, buffers[freeBufferID]);
        }

        public void Close()
        {
            FillWithSilence();
            bool wait = true;
            while (wait)
            {
                AL.GetSource(source, ALGetSourcei.SourceState, out int sourceState);
                if ((ALSourceState)sourceState != ALSourceState.Playing)
                {
                    wait = false;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
            ///Dispose
            if (context != ALContext.Null)
            {
                ALC.MakeContextCurrent(ALContext.Null);
                ALC.DestroyContext(context);
            }
            context = ALContext.Null;

            if (device != ALDevice.Null)
            {
                ALC.CloseDevice(device);
            }
            device = ALDevice.Null;
        }
    }
}