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

        // Does stop record get uploaded to server straight or saved locally?
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
