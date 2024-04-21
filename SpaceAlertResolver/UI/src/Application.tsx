import React from 'react';

export enum IWorkflowState {
	Initial,
	CameraStarting,
	CameraStarted,
	Done,
	Error
}

export default function Application() {
	return <div>Hello, world!</div>;
}
