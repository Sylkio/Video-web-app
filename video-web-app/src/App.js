import React, { useState } from 'react';
import VideoUpload from './components/VideoUpload';
import VideoPlayer from './components/VideoPlayer';

function App() {
    const [currentVideoUrl, setCurrentVideoUrl] = useState(null);


    const handleVideoSelect = (videoUrl) => {
        setCurrentVideoUrl(videoUrl);
    };

    return (
        <div className="App">
            <h1>Video Web Application</h1>
            <VideoUpload onVideoSelect={handleVideoSelect} />
            <VideoPlayer videoUrl={currentVideoUrl} />
        </div>
    );
}

export default App;