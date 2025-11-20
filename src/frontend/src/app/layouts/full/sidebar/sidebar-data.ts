import { NavItem } from './nav-item/nav-item';

export const navItems: NavItem[] = [
  {
    navCap: 'Dashboard',
  },
  {
    displayName: 'Dashboard',
    iconName: 'material-symbols-light:dashboard-outline',
    route: 'pages/dashboard',
  },
  {
    navCap: 'Conteúdo',
    divider: true
  },
  {
    displayName: 'Cursos',
    iconName: 'material-symbols-light:library-books-outline',
    route: 'pages/cursos',
  },
  {
    navCap: 'Area do Aluno',
    divider: true
  },
  {
    displayName: 'Minhas matrículas',
    iconName: 'material-symbols-light:school-outline',
    route: 'pages/matriculas',
  },
  {
    displayName: 'Certificados',
    iconName: 'material-symbols-light:verified-outline',
    route: 'pages/certificados',
  }
];
