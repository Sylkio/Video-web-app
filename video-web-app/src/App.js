import React, { useState } from 'react'; // Import useState here
import VideoUpload from './components/VideoUpload';
import VideoList from './components/VideoList';
import './VideoList.css';

function App() {
    const [currentVideoUrl, setCurrentVideoUrl] = useState(null);

    const handleVideoSelect = (videoUrl) => {
        setCurrentVideoUrl(videoUrl);
    };

    return (
        <div className="App">
            <h1>Video Web Application</h1>
            <VideoUpload onVideoSelect={handleVideoSelect} />
            <VideoList videoUrl={currentVideoUrl} />
        </div>
    );
}

export default App;
