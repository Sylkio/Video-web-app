import React, { useState, useRef } from 'react';

function VideoRecord() {
    const [recording, setRecording] = useState(false);
    const videoRef = useRef();
    const mediaRecorderRef = useRef();

    const startRecording = async () => {
        const stream = await navigator.mediaDevices.getUserMedia({ video: true });
        videoRef.current.srcObject = stream;
        mediaRecorderRef.current = new MediaRecorder(stream);

        mediaRecorderRef.current.start();
        setRecording(true);
    };

    const stopRecording = () => {
        mediaRecorderRef.current.stop();
        setRecording(false);

        // Handle the recorded video here
        // For example, you can save it or upload it to the server
    };

    return (
        <div>
            <video ref={videoRef} autoPlay muted></video>
            {recording ? (
                <button onClick={stopRecording}>Stop Recording</button>
            ) : (
                <button onClick={startRecording}>Start Recording</button>
            )}
        </div>
    );
}

export default VideoRecord;
