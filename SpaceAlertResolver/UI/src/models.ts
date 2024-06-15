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
	scannedCards: ScannedCard[];
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
	scannedCards?: ScannedCard[];
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

export type WholeCard = { type: 'whole'; front: string; back: string };
export type SplitCard = {
	type: 'split';
	top: string;
	bottom: string;
	back: string;
	orientation: Orientation;
};
export type SkippedCard = null;

export type ScannedCard = WholeCard | SplitCard | SkippedCard;

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
