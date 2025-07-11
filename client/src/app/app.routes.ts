import { Routes } from '@angular/router';
import { Home } from '../features/home/home';
import { MemberList } from '../features/members/member-list/member-list';
import { Lists } from '../features/lists/lists';
import { Messages } from '../features/messages/messages';
import { MemberDetailed } from '../features/members/member-detailed/member-detailed';
import { authGuard } from '../core/guards/auth-guard';

export const routes: Routes = [
    {path: '', component: Home},
    {path: 'members', component: MemberList, canActivate: [authGuard]},
    {path: 'members/:id', component: MemberDetailed },
    {path: 'lists', component: Lists},
    {path: 'messages', component: Messages},
    {path: '**', component: Home}
];
