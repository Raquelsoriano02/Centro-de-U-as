import { api, loadAll } from './api.js';
import { ESTADOS, estadoBadge } from './modules.js';
import { emptyState, errorState, formatTime, fullName, h, icon, money, skeleton, today } from './ui.js';

function statTile(iconName, label, value, note) {
  return h('article', { class: 'card stat' },
    h('div', { class: 'stat-label' }, icon(iconName), label),
    h('div', { class: 'stat-value' }, value),
    note ? h('div', { class: 'stat-note' }, note) : null);
}

export function renderDashboard(root) {
  async function load() {
    root.replaceChildren(h('div', { class: 'card' }, skeleton()));

    try {
      const hoy = today();
      const [data, deHoy] = await Promise.all([
        loadAll(['turnos', 'clientes', 'tecnicas', 'servicios', 'pagos']),
        api.turnosDelDia(hoy),
      ]);
      const { turnos, clientes, tecnicas, servicios, pagos } = data;
      const ahora = new Date();
      const mes = hoy.slice(0, 7);
      const pagosMes = pagos.filter(p => p.fechaPago.startsWith(mes));
      const cobradoMes = pagosMes.reduce((suma, p) => suma + p.monto, 0);
      const porConfirmar = turnos.filter(t => t.estado === 'Pendiente').length;
      const nombreDe = (lista, id) => fullName(lista.find(x => x.id === id));
      const servicioDe = id => servicios.find(s => s.id === id)?.nombre ?? '—';
      const maximo = Math.max(1, ...Object.keys(ESTADOS).map(e => turnos.filter(t => t.estado === e).length));

      const agendaHost = h('div');
      const renderAgenda = lista => agendaHost.replaceChildren(lista.length
        ? h('ul', { class: 'agenda' }, lista.map(t => h('li', {},
          h('div', { class: 'agenda-time' },
            h('strong', {}, formatTime(t.fechaHoraInicio)),
            h('span', { class: 'muted small' }, formatTime(t.fechaHoraFin))),
          h('div', { class: 'agenda-main' },
            h('strong', {}, nombreDe(clientes, t.clienteId)),
            h('span', { class: 'muted small' }, `${servicioDe(t.servicioId)} · ${nombreDe(tecnicas, t.tecnicaId)}`)),
          estadoBadge(t.estado))))
        : emptyState('calendar', 'No hay turnos para hoy', 'Los turnos agendados para hoy aparecerán aquí.'));

      const filtroTecnica = h('select', {
        class: 'compact',
        'aria-label': 'Filtrar por técnica',
        onchange: async e => {
          agendaHost.replaceChildren(skeleton());
          try {
            renderAgenda(await api.turnosDelDia(hoy, e.target.value));
          } catch (error) {
            agendaHost.replaceChildren(errorState(error.message, load));
          }
        },
      },
        h('option', { value: '' }, 'Todas las técnicas'),
        tecnicas.map(t => h('option', { value: t.id }, fullName(t))));

      renderAgenda(deHoy);

      root.replaceChildren(
        h('div', { class: 'page-head' }, h('div', {},
          h('h1', {}, 'Inicio'),
          h('p', {}, new Intl.DateTimeFormat('es', { dateStyle: 'full' }).format(ahora)))),
        h('section', { class: 'stats', 'aria-label': 'Resumen' },
          statTile('users', 'Clientes', String(clientes.length), 'registrados'),
          statTile('calendar', 'Turnos de hoy', String(deHoy.filter(t => t.estado !== 'Cancelado').length), 'sin contar cancelados'),
          statTile('clock', 'Por confirmar', String(porConfirmar), 'turnos pendientes'),
          statTile('money', 'Cobrado este mes', money(cobradoMes), `${pagosMes.length} pago(s)`)),
        h('div', { class: 'dash-grid' },
          h('section', { class: 'card' },
            h('div', { class: 'card-head' },
              h('h2', {}, 'Agenda de hoy'),
              h('div', { class: 'card-tools' }, filtroTecnica, h('a', { href: '#/turnos' }, 'Ver todos'))),
            agendaHost),
          h('section', { class: 'card' },
            h('div', { class: 'card-head' }, h('h2', {}, 'Turnos por estado')),
            h('div', { class: 'bars' }, Object.keys(ESTADOS).map(estado => {
              const total = turnos.filter(t => t.estado === estado).length;
              return h('div', { title: `${estado}: ${total}` },
                h('div', { class: 'bar-head' }, h('span', {}, estado), h('strong', {}, String(total))),
                h('div', { class: 'bar-track' },
                  total ? h('div', { class: 'bar-fill', style: `width:${(total / maximo) * 100}%` }) : null));
            })))),
      );
    } catch (error) {
      root.replaceChildren(h('div', { class: 'card' }, errorState(error.message, load)));
    }
  }

  load();
}
