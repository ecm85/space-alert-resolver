import { Button, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { BarcodeScanningWorkflow } from '~/components/BarcodeScanningWorkflow';
import { ColorPicker } from '~/components/ColorPicker';
import { PlayerBoard } from '~/components/PlayerBoard';
import { useBarcodeData } from '~/hooks';
import { useConnectionSubscription } from '~/hooks/useConnectionSubscription';
import { MessageEventData, PlayerColor, InputCardsWorkflowState as WorkflowState } from '~/models';
import styles from './InputCards.module.css';
import { DetectedBarcode } from 'barcode-detector';

export interface InputCardsProps {
	gameCode: string;
	connection: WebSocket | null;
}

export function InputCards({ gameCode, connection }: InputCardsProps) {
	const [workflowState, setWorkflowState] = useState(WorkflowState.Scanning);
	const [barcodes, setBarcodes] = useState<DetectedBarcode[]>([]);
	const [message, setMessage] = useState('');
	const { barcodeData, playerColor: deducedPlayerColor } = useBarcodeData({ barcodes });
	const [manualPlayerColor, setManualPlayerColor] = useState<PlayerColor | null>(null);

	const handleBarcodesScanned = (newBarcodes: DetectedBarcode[]) => {
		setBarcodes(newBarcodes);
	};

	useEffect(() => {
		if (workflowState === WorkflowState.Scanning && barcodeData.length > 0) {
			setWorkflowState(
				deducedPlayerColor == null ? WorkflowState.ChooseColor : WorkflowState.Scanned,
			);
		}
	}, [workflowState, barcodeData, deducedPlayerColor]);

	const handleScanAgainClicked = () => {
		setBarcodes([]);
		setWorkflowState(WorkflowState.Scanning);
	};

	const handleMessage = (messageEventData: MessageEventData) => {
		switch (messageEventData.event) {
			case 'YourMessageSent':
				setMessage('Your cards have been sent to the host.');
				setWorkflowState(WorkflowState.Uploaded);
				return true;
			default:
				return false;
		}
	};

	const playerColor = manualPlayerColor != null ? manualPlayerColor : deducedPlayerColor;

	const handleSendToServerClicked = () => {
		const data = {
			barcodeData,
			playerColor,
		};
		connection?.send(JSON.stringify({ action: 'SendToHost', data: { code: gameCode, data } }));
		setWorkflowState(WorkflowState.Uploading);
	};

	const { isSubscribed } = useConnectionSubscription({
		connection,
		onMessage: handleMessage,
		connectionStarted: true,
	});

	const canRescanStates = [WorkflowState.Scanned, WorkflowState.Uploaded];

	const hasNotAlreadyScanned = workflowState === WorkflowState.Scanning;
	const hasAlreadyScanned = !hasNotAlreadyScanned;

	const handleColorPicked = () => {
		// TODO:
		setManualPlayerColor(PlayerColor.Blue);
	};

	return (
		<div className={styles['root']}>
			<h2>GameCode: {gameCode}</h2>
			{message && <div>{message}</div>}

			{hasNotAlreadyScanned && <BarcodeScanningWorkflow onBarcodesScan={handleBarcodesScanned} />}
			{hasAlreadyScanned && (
				<div>
					<PlayerBoard barcodeData={barcodeData} playerColor={playerColor} />
					{workflowState === WorkflowState.ChooseColor && (
						<ColorPicker onPickColor={handleColorPicked} value={manualPlayerColor} />
					)}
					{canRescanStates && (
						<div>
							<Button onClick={handleScanAgainClicked}>Scan Again</Button>
						</div>
					)}
					{isSubscribed && (
						<div>
							{workflowState === WorkflowState.Scanned && (
								<Button onClick={handleSendToServerClicked} variant="contained">
									Submit Cards
								</Button>
							)}
							{workflowState === WorkflowState.Uploading && (
								<Button variant="contained" disabled>
									Submitting...
								</Button>
							)}
							{workflowState === WorkflowState.ErrorUploading && (
								<Typography variant="body1">There was an error submitting your cards.</Typography>
							)}
							{workflowState === WorkflowState.Uploaded && (
								<Typography variant="body1">Your cards have been submitted.</Typography>
							)}
						</div>
					)}
				</div>
			)}
		</div>
	);
}
