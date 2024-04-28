import React from 'react';
import { useNavigate } from 'react-router-dom';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuIcon from '@mui/icons-material/Menu';
import Container from '@mui/material/Container';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import RocketLaunchIcon from '@mui/icons-material/RocketLaunch';
import { Outlet } from 'react-router-dom';

export function PageFrame() {
	const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(null);

	const navigate = useNavigate();

	const handleCreateGameClicked = () => {
		navigate('/create');
		handleCloseNavMenu();
	};
	const handleJoinGameClicked = () => {
		navigate('/join');
		handleCloseNavMenu();
	};
	const pages = [
		{ name: 'Create Game', handler: handleCreateGameClicked },
		{ name: 'Join Game', handler: handleJoinGameClicked }
	];

	const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
		setAnchorElNav(event.currentTarget);
	};

	const handleCloseNavMenu = () => {
		setAnchorElNav(null);
	};

	return (
		<>
			<AppBar position='static'>
				<Container maxWidth='xl'>
					<Toolbar disableGutters>
						<RocketLaunchIcon sx={{ display: { xs: 'none', md: 'flex' }, mr: 1 }} />
						<Typography
							variant='h6'
							noWrap
							component='a'
							href='/'
							sx={{
								mr: 2,
								display: { xs: 'none', md: 'flex' },
								fontFamily: 'monospace',
								fontWeight: 700,
								letterSpacing: '.3rem',
								color: 'inherit',
								textDecoration: 'none'
							}}>
							Space Alert
						</Typography>

						<Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
							<IconButton size='large' onClick={handleOpenNavMenu} color='inherit'>
								<MenuIcon />
							</IconButton>
							<Menu
								anchorEl={anchorElNav}
								anchorOrigin={{
									vertical: 'bottom',
									horizontal: 'left'
								}}
								keepMounted
								transformOrigin={{
									vertical: 'top',
									horizontal: 'left'
								}}
								open={Boolean(anchorElNav)}
								onClose={handleCloseNavMenu}
								sx={{
									display: { xs: 'block', md: 'none' }
								}}>
								{pages.map(page => (
									<MenuItem key={page.name} onClick={page.handler}>
										<Typography textAlign='center'>{page.name}</Typography>
									</MenuItem>
								))}
							</Menu>
						</Box>
						<RocketLaunchIcon sx={{ display: { xs: 'flex', md: 'none' }, mr: 1 }} />
						<Typography
							variant='h5'
							noWrap
							component='a'
							href='/'
							sx={{
								mr: 2,
								display: { xs: 'flex', md: 'none' },
								flexGrow: 1,
								fontFamily: 'monospace',
								fontWeight: 700,
								letterSpacing: '.3rem',
								color: 'inherit',
								textDecoration: 'none'
							}}>
							Space Alert
						</Typography>
						<Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
							{pages.map(page => (
								<Button
									key={page.name}
									onClick={page.handler}
									sx={{ my: 2, color: 'white', display: 'block' }}>
									{page.name}
								</Button>
							))}
						</Box>
					</Toolbar>
				</Container>
			</AppBar>
			<Container maxWidth='xl'>
				<Outlet />
			</Container>
		</>
	);
}
