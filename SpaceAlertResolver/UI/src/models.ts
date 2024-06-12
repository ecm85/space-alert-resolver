export interface CreatedGameEvent {
	event: 'GameCreated';
	data: {
		code: string;
	};
}

export interface InvalidGameCodeEvent {
	event: 'InvalidGameCode';
}

export interface ExpiredGameCodeEvent {
	event: 'ExpiredGameCode';
}

export interface YouJoinedEvent {
	event: 'YouJoined';
}

export interface ClientJoinedEvent {
	event: 'ClientJoined';
	data: {
		name: string;
		connectionId: string;
	};
}

export interface ClientMessageReceivedEvent {
	event: 'ClientMessageReceived';
	data: {
		connectionId: string;
		messageType: string;
		message: string;
	};
}

export interface PlayerInput {
	barcodeData: ParsedBarcode[];
	playerColor: PlayerColor | null;
}

export interface YourMessageSentEvent {
	event: 'YourMessageSent';
}

export type MessageEventData =
	| CreatedGameEvent
	| InvalidGameCodeEvent
	| ExpiredGameCodeEvent
	| YouJoinedEvent
	| ClientJoinedEvent
	| ClientMessageReceivedEvent
	| YourMessageSentEvent;

export interface Client {
	name: string;
	connectionId: string;
	barcodeData?: ParsedBarcode[];
	playerColor?: PlayerColor | null;
}

export enum PlayerColor {
	Red = 0,
	Blue,
	Green,
	Yellow,
	Purple,
}

export enum Orientation {
	Top,
	Bottom,
	Other,
}

export type ParsedBarcode = string | null;

export enum BarcodeScanningWorkflowState {
	Initial,
	CameraStarting,
	CameraStarted,
	Error,
}

export enum InputCardsWorkflowState {
	Scanning,
	Scanned,
	ChooseColor,
	Uploading,
	ErrorUploading,
	Uploaded,
}
