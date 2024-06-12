import { Button, Typography } from '@mui/material';
import { MutableRefObject, useCallback, useState } from 'react';
import { BarcodeScanningWorkflow } from '~/components/BarcodeScanningWorkflow';
import { ColorPicker } from '~/components/ColorPicker';
import { PlayerBoard } from '~/components/PlayerBoard';
import { useBarcodeData } from '~/hooks';
import { useConnectionSubscription } from '~/hooks/useConnectionSubscription';
import { MessageEventData, PlayerColor, InputCardsWorkflowState as WorkflowState } from '~/models';
import styles from './InputCards.module.css';
import { DetectedBarcode } from 'barcode-detector';
import { useSendToHostMessaging } from '~/hooks/useSendToHostMessaging';

export interface InputCardsProps {
	gameCode: string;
	connectionRef: MutableRefObject<WebSocket | null>;
}

export function InputCards({ gameCode, connectionRef }: InputCardsProps) {
	const [workflowState, setWorkflowState] = useState(WorkflowState.Scanning);
	const [barcodes, setBarcodes] = useState<DetectedBarcode[]>([]);
	const [message, setMessage] = useState('');
	const { barcodeData, playerColor: deducedPlayerColor } = useBarcodeData({ barcodes });
	const [manualPlayerColor, setManualPlayerColor] = useState<PlayerColor | null>(null);
	const { serialize } = useSendToHostMessaging();

	const handleBarcodesScanned = (newBarcodes: DetectedBarcode[]) => {
		setBarcodes(newBarcodes);
		setWorkflowState(
			deducedPlayerColor == null ? WorkflowState.ChooseColor : WorkflowState.Scanned,
		);
	};

	const handleScanAgainClicked = () => {
		setBarcodes([]);
		setManualPlayerColor(null);
		setWorkflowState(WorkflowState.Scanning);
	};

	const handleMessage = useCallback(
		(messageEventData: MessageEventData) => {
			switch (messageEventData.event) {
				case 'YourMessageSent':
					setMessage('Your cards have been sent to the host.');
					setWorkflowState(WorkflowState.Uploaded);
					return true;
				default:
					return false;
			}
		},
		[setMessage, setWorkflowState],
	);

	const playerColor = manualPlayerColor != null ? manualPlayerColor : deducedPlayerColor;

	const handleSendToServerClicked = () => {
		const data = {
			barcodeData,
			playerColor,
		};
		const { message, messageType } = serialize(data);
		connectionRef.current?.send(
			JSON.stringify({
				action: 'SendToHost',
				data: { code: gameCode, message, messageType },
			}),
		);
		setWorkflowState(WorkflowState.Uploading);
	};

	const { isSubscribed } = useConnectionSubscription({
		connectionRef,
		onMessage: handleMessage,
		connectionStarted: true,
	});

	const canRescanStates = [WorkflowState.Scanned, WorkflowState.Uploaded];

	const hasNotAlreadyScanned = workflowState === WorkflowState.Scanning;
	const hasAlreadyScanned = !hasNotAlreadyScanned;

	const handleColorPicked = (newManualPlayerColor: PlayerColor) => {
		setManualPlayerColor(newManualPlayerColor);
		setWorkflowState(WorkflowState.Scanned);
	};

	return (
		<div className={styles['root']}>
			<h2>GameCode: {gameCode}</h2>
			{message && <div>{message}</div>}

			{hasNotAlreadyScanned && <BarcodeScanningWorkflow onBarcodesScan={handleBarcodesScanned} />}
			{hasAlreadyScanned && (
				<div className={styles['already-scanned']}>
					<ColorPicker onPickColor={handleColorPicked} value={manualPlayerColor} />
					<PlayerBoard barcodeData={barcodeData} playerColor={playerColor} />
					<div>
						{isSubscribed && workflowState === WorkflowState.Scanned && (
							<Button onClick={handleSendToServerClicked} variant="contained">
								Submit Cards
							</Button>
						)}
						{workflowState === WorkflowState.Uploading && (
							<Button variant="contained" disabled>
								Submitting...
							</Button>
						)}
						{canRescanStates && (
							<Button onClick={handleScanAgainClicked} color="secondary">
								Scan Again
							</Button>
						)}
						{workflowState === WorkflowState.ErrorUploading && (
							<Typography variant="body1">There was an error submitting your cards.</Typography>
						)}
						{workflowState === WorkflowState.Uploaded && (
							<Typography variant="body1">Your cards have been submitted.</Typography>
						)}
					</div>
				</div>
			)}
		</div>
	);
}
